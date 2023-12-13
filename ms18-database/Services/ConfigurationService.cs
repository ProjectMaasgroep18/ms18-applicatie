using Microsoft.Extensions.Configuration;

namespace Maasgroep.Database.Services
{
	public class ConfigurationService
	{
		private readonly IConfiguration _configuration;
		public ConfigurationService(IConfiguration configuration) 
		{ 
			_configuration = configuration;
		}

		public string GetConnectionString()
		{
			return "";
		}
	}
}
