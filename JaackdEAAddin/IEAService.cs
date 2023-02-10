namespace JaackdEAAddin
{
    internal interface IEAService
    {
        EA.Repository GetRepository();
        string GetEventPropertiesString(EA.EventProperties eventProperties);
    }
}
