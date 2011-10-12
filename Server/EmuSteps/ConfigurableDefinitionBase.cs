namespace WindowsPhoneTestFramework.EmuSteps
{
    public class ConfigurableDefinitionBase
    {
        private IConfiguration _configuration;

        protected IConfiguration Configuration { get { return _configuration; } }

        public ConfigurableDefinitionBase()
            : this(new AppConfigFileBasedConfiguration())
        {            
        }

        public ConfigurableDefinitionBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}