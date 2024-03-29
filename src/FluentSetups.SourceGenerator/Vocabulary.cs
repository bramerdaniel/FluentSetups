﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vocabulary.cs" company="consolovers">
//   Copyright (c) daniel bramer 2022 - 2022
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace FluentSetups.SourceGenerator
{
   using System.Collections.Generic;
   using System.Diagnostics.CodeAnalysis;
   using System.Text.RegularExpressions;

   /// <summary>
   ///    A container for exceptions to simple pluralization/singularization rules. Vocabularies.Default contains an extensive list of rules for US
   ///    English. At this time, multiple vocabularies and removing existing rules are not supported.
   /// </summary>
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   [SuppressMessage("ReSharper", "CommentTypo")]
   [SuppressMessage("ReSharper", "IdentifierTypo")]
   public class Vocabulary
   {
      #region Constants and Fields

      private readonly Regex letterS = new Regex("^([sS])[sS]*$");

      private readonly List<Rule> plurals = new List<Rule>();

      private readonly List<Rule> singulars = new List<Rule>();

      private readonly List<string> uncountable = new List<string>();

      #endregion

      #region Constructors and Destructors

      internal Vocabulary()
      {
      }

      #endregion

      #region Public Methods and Operators

      /// <summary>Adds a word to the vocabulary which cannot easily be pluralized/singularized by RegEx, e.g. "person" and "people".</summary>
      /// <param name="singular">The singular form of the irregular word, e.g. "person".</param>
      /// <param name="plural">The plural form of the irregular word, e.g. "people".</param>
      /// <param name="matchEnding">True to match these words on their own as well as at the end of longer words. False, otherwise.</param>
      public void AddIrregular(string singular, string plural, bool matchEnding = true)
      {
         if (matchEnding)
         {
            AddPlural("(" + singular[0] + ")" + singular.Substring(1) + "$", "$1" + plural.Substring(1));
            AddSingular("(" + plural[0] + ")" + plural.Substring(1) + "$", "$1" + singular.Substring(1));
         }
         else
         {
            AddPlural($"^{singular}$", plural);
            AddSingular($"^{plural}$", singular);
         }
      }

      /// <summary>Adds a rule to the vocabulary that does not follow trivial rules for pluralization, e.g. "bus" -> "buses"</summary>
      /// <param name="rule">RegEx to be matched, case insensitive, e.g. "(bus)es$"</param>
      /// <param name="replacement">RegEx replacement  e.g. "$1"</param>
      public void AddPlural(string rule, string replacement)
      {
         plurals.Add(new Rule(rule, replacement));
      }

      /// <summary>Adds a rule to the vocabulary that does not follow trivial rules for singularization, e.g. "vertices/indices -> "vertex/index"</summary>
      /// <param name="rule">RegEx to be matched, case insensitive, e.g. ""(vert|ind)ices$""</param>
      /// <param name="replacement">RegEx replacement  e.g. "$1ex"</param>
      public void AddSingular(string rule, string replacement)
      {
         singulars.Add(new Rule(rule, replacement));
      }

      /// <summary>Adds an uncountable word to the vocabulary, e.g. "fish".  Will be ignored when plurality is changed.</summary>
      /// <param name="word">Word to be added to the list of uncountable.</param>
      public void AddUncountable(string word)
      {
         uncountable.Add(word.ToLower());
      }

      /// <summary>Pluralizes the provided input considering irregular words</summary>
      /// <param name="word">Word to be pluralized</param>
      /// <param name="inputIsKnownToBeSingular">Normally you call Pluralize on singular words; but if you're unsure call it with false</param>
      /// <returns></returns>
      public string Pluralize(string word, bool inputIsKnownToBeSingular = true)
      {
         var s = LetterS(word);
         if (s != null)
            return $"{s}s";

         var result = ApplyRules(plurals, word, false);

         if (inputIsKnownToBeSingular)
            return result ?? word;

         var asSingular = ApplyRules(singulars, word, false);
         var asSingularAsPlural = ApplyRules(plurals, asSingular, false);
         if (asSingular != null && asSingular != word && asSingular + "s" != word && asSingularAsPlural == word && result != word)
            return word;

         return result;
      }

      /// <summary>Singularizes the provided input considering irregular words</summary>
      /// <param name="word">Word to be singularized</param>
      /// <param name="inputIsKnownToBePlural">Normally you call Singularize on plural words; but if you're unsure call it with false</param>
      /// <param name="skipSimpleWords">Skip singularizing single words that have an 's' on the end</param>
      /// <returns>The singularized word</returns>
      public string Singularize(string word, bool inputIsKnownToBePlural = true, bool skipSimpleWords = false)
      {
         var s = LetterS(word);
         if (s != null)
            return s;

         var result = ApplyRules(singulars, word, skipSimpleWords);

         if (inputIsKnownToBePlural)
            return result ?? word;

         // the Plurality is unknown so we should check all possibilities
         var asPlural = ApplyRules(plurals, word, false);
         var asPluralAsSingular = ApplyRules(singulars, asPlural, false);
         if (asPlural != word && word + "s" != asPlural && asPluralAsSingular == word && result != word)
         {
            return word;
         }

         return result ?? word;
      }

      #endregion

      #region Methods

      private string ApplyRules(IList<Rule> rules, string word, bool skipFirstRule)
      {
         if (word == null)
            return null;

         if (word.Length < 1)
            return word;

         if (IsUncountable(word))
            return word;

         var result = word;
         var end = skipFirstRule ? 1 : 0;
         for (var i = rules.Count - 1; i >= end; i--)
         {
            if ((result = rules[i].Apply(word)) != null)
               break;
         }

         return result != null ? MatchUpperCase(word, result) : null;
      }

      private bool IsUncountable(string word)
      {
         return uncountable.Contains(word.ToLower());
      }

      /// <summary>If the word is the letter s, singular or plural, return the letter s singular</summary>
      private string LetterS(string word)
      {
         var s = letterS.Match(word);
         return s.Groups.Count > 1 ? s.Groups[1].Value : null;
      }

      private string MatchUpperCase(string word, string replacement)
      {
         return char.IsUpper(word[0]) && char.IsLower(replacement[0]) ? char.ToUpper(replacement[0]) + replacement.Substring(1) : replacement;
      }

      #endregion

      private sealed class Rule
      {
         #region Constants and Fields

         private readonly Regex regex;

         private readonly string replacement;

         #endregion

         #region Constructors and Destructors

         public Rule(string pattern, string replacement)
         {
            regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.replacement = replacement;
         }

         #endregion

         #region Public Methods and Operators

         public string Apply(string word)
         {
            return !regex.IsMatch(word) ? null : regex.Replace(word, replacement);
         }

         #endregion
      }
   }
}