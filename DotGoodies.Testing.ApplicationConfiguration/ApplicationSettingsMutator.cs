using System;
using System.Configuration;
using System.Linq.Expressions;

namespace DotGoodies.Testing.ApplicationConfiguration
{
    public sealed class ApplicationSettingsMutator<TSettings> 
        where TSettings : ApplicationSettingsBase
    {
        private readonly TSettings _settings;

        public ApplicationSettingsMutator(TSettings settings)
        {
            _settings = settings;
        }

        public ApplicationSettingsMutator<TSettings> Override<TValue>(Expression<Func<TSettings, TValue>> property, TValue value)
        {
            if (!(property.Body is MemberExpression)) 
                throw new InvalidOperationException("Unable to figure out property name.");

            _settings.Reload();

            var member = property.Body as MemberExpression;

            var _ = _settings[member.Member.Name];

            _settings.PropertyValues[member.Member.Name].PropertyValue = value;

            return this;
        }

        public void Reload()
        {
            _settings.Reset();
            _settings.Reload();
        }
    }
}
