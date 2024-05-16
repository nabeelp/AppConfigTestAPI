using Microsoft.FeatureManagement.FeatureFilters;
using Microsoft.FeatureManagement;

namespace AppConfigTestAPI
{
    public interface IFeatureFlagProvider
    {
        Task<bool> IsEnabled(string feature);
        Task<IDictionary<string, bool>> GetAll();
    }

    public class AzureFeatureFlagProvider : IFeatureFlagProvider
    {
        private readonly IFeatureManager _featureManager;
        private readonly ITargetingContextAccessor _targetingContextAccessor;

        public AzureFeatureFlagProvider(IFeatureManager featureManager, ITargetingContextAccessor targetingContextAccessor)
        {
            _featureManager = featureManager;
            _targetingContextAccessor = targetingContextAccessor;
        }

        public async Task<bool> IsEnabled(string feature)
        {
            return await _featureManager.IsEnabledAsync(feature, _targetingContextAccessor);
        }

        public async Task<IDictionary<string, bool>> GetAll()
        {
            var features = new Dictionary<string, bool>();
            var featureNames = _featureManager.GetFeatureNamesAsync();
            await foreach (var featureName in featureNames)
            {
                features.Add(featureName, await _featureManager.IsEnabledAsync(featureName, _targetingContextAccessor));
            }

            return features;
        }
    }
}
