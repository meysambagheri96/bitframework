﻿using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bit.Core.Models;
using System;

namespace Bit.IdentityServer.Implementations
{
    public class RegexBasedRedirectUriValidator : DefaultRedirectUriValidator, IRedirectUriValidator
    {
        public virtual AppEnvironment AppEnvironment { get; set; } = default!;

        public override async Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return AppEnvironment.DebugMode == true ||
                (await base.IsPostLogoutRedirectUriValidAsync(requestedUri, client).ConfigureAwait(false)) ||
                client.PostLogoutRedirectUris.Any(rUri => Regex.IsMatch(requestedUri, rUri, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant));
        }

        public override async Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            return AppEnvironment.DebugMode == true ||
                (await base.IsRedirectUriValidAsync(requestedUri, client).ConfigureAwait(false)) ||
                client.RedirectUris.Any(rUri => Regex.IsMatch(requestedUri, rUri, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant));
        }
    }
}
