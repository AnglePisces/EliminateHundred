using System;
namespace Tools
{
	public delegate T CallBackReturn<T>();

	public delegate void CallBack();

	public delegate void CallBackArg<T>(T t);

	public delegate void CallBackArg<T, U>(T t, U u);

	public delegate void CallBackArg<T, U, K>(T t, U u, K k);

	public delegate void CallBackArg<T, U, K, J>(T t, U u, K k, J j);

	public delegate void CallBackArg<T, U, K, J, V>(T t, U u, K k, J j, V v);

	public delegate void CallBackArg<T1, T2, T3, T4, T5, T6>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6);

	public delegate void CallBackArg<T1, T2, T3, T4, T5, T6, T7>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7);

	public delegate void CallBackArg<T1, T2, T3, T4, T5, T6, T7, T8>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8);

	public delegate void CallBackArg<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, T9 p9);

	public delegate object CallBackArgRturn<T>(T t);

	public delegate object CallBackReturn();
}
