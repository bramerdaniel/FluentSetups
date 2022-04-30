namespace FluentSetups
{
   public interface IFluentSetup<out T>
   {
      T Done();
   }
}