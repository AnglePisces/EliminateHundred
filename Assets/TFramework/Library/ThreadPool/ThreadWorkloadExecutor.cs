namespace Frankfort.Threading.Internal
{
    public delegate void ThreadWorkloadExecutorIndexed<T>(T workload, int workloadIndex);

    public delegate void ThreadWorkloadExecutor<T>(T workload);

    public delegate void ThreadWorkloadExecutorArg<T>(T workload, object extraArgument);

    public delegate void ThreadWorkloadExecutorArgIndexed<T>(T workload, int workloadIndex, object extraArgument);
}
