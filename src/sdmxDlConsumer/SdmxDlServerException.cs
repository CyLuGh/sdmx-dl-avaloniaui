namespace sdmxDlConsumer
{
    [Serializable]
    public class SdmxDlServerException : ApplicationException
    {
        public SdmxDlServerException() : base()
        {
        }

        public SdmxDlServerException( string? message ) : base( message )
        {
        }

        public SdmxDlServerException( string? message , Exception? innerException ) : base( message , innerException )
        {
        }
    }
}