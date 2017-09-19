using System;
namespace Frankfort.Threading.Internal
{
	public interface DispatchAction
	{
		bool Executed
		{
			get;
		}
		void ExecuteDispatch();
	}
}
