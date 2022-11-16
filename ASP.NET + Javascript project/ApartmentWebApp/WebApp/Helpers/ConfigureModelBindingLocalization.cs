using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApp.Helpers;

public class ConfigureModelBindingLocalization : IConfigureOptions<MvcOptions>
{
    public void Configure(MvcOptions options)
    {
        options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
            string.Format(Base.Resources.Errors.ErrorMessage_AttemptedValueIsInvalid, x, y));

        options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) =>
            string.Format(Base.Resources.Errors.ErrorMessage_MissingBindRequiredValue, x));

        // localizer["A value for the '{0}' parameter or property was not provided.", x]);

        options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() =>
            Base.Resources.Errors.ErrorMessage_MissingKeyOrValue);

        // localizer["A value is required."]);

        options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() =>
            Base.Resources.Errors.ErrorMessage_MissingRequestBodyRequiredValue);

        // localizer["A non-empty request body is required."]);

        options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) =>
            string.Format(Base.Resources.Errors.ErrorMessage_NonPropertyAttemptedValueIsInvalid, x));
        // localizer["The value '{0}' is not valid.", x]);

        options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() =>
            Base.Resources.Errors.ErrorMessage_NonPropertyUnknownValueIsInvalid);
        // localizer["The supplied value is invalid."]);

        options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() =>
            Base.Resources.Errors.ErrorMessage_NonPropertyValueMustBeANumber);
        // localizer["The field must be a number."]);

        options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) =>
            string.Format(Base.Resources.Errors.ErrorMessage_UnknownValueIsInvalid, x));
        //  localizer["The supplied value is invalid for {0}.", x]);

        options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) =>
            string.Format(Base.Resources.Errors.ErrorMessage_ValueIsInvalid, x));
        //  localizer["The value '{0}' is invalid.", x]);

        options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) =>
            string.Format(Base.Resources.Errors.ErrorMessage_ValueMustBeANumber, x));
        //  localizer["The field {0} must be a number.", x]);

        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) =>
            string.Format(Base.Resources.Errors.ErrorMessage_ValueMustNotBeNull, x));
        //  localizer["The value '{0}' is invalid.", x]);
    }
}