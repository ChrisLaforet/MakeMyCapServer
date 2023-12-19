using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MakeMyCapServer.Security.JWT
{
    public class JsonWebToken
    {
        public const string JWT_SIGNATURE_ALGORITHM = "HS256";
        public const string JWT_ISSUER = "MMCServer";

        public const string SERIAL_CLAIM = "ser";
        public const string USER_NAME_CLAIM = "user";
        public const string USER_ROLE_CLAIM = "roles";

        private string tokenString;
        private string jwtIssuer;

        private string algorithm;
        private string user;
        private string subject;
        private string serial;
        private string issuer;
        private string userId;
        private List<string> roles = new List<string>();
        private Dictionary<string, Claim> claims = new Dictionary<string, Claim>();
        private DateTime issuedAt;
        private DateTime expiresAt;

        private JsonWebToken(string tokenString, string jwtIssuer)
        {
            if (tokenString == null || tokenString.Length == 0)
            {
                throw new ArgumentException("A JWT cannot be empty or null");
            }

            this.tokenString = tokenString;
            this.jwtIssuer = jwtIssuer;
            decodeJWT(tokenString);
        }

        private void decodeJWT(string tokenString)
        {
            JwtSecurityToken contents;
            try
			{
                contents = new JwtSecurityToken(tokenString);
            } 
            catch (Exception ex)
			{
                throw new JWTDecodeException("Error decoding token string", ex);
			}

            algorithm = contents.SignatureAlgorithm;
            if (algorithm == null || algorithm != JWT_SIGNATURE_ALGORITHM)
            {
                throw new AuthenticationException("JWT with an invalid issuer");
            }
            
            issuer = contents.Issuer;
            if (issuer == null || issuer != jwtIssuer)
            {
                throw new AuthenticationException("JWT with an invalid issuer");
            }

            issuedAt = contents.IssuedAt;
            if (issuedAt.Ticks == 0)
            {
                throw new AuthenticationException("JWT with a missing issued timestamp");
            }

            expiresAt = contents.ValidTo;
            if (expiresAt.Ticks == 0)
            {
                throw new AuthenticationException("JWT with a missing expiration");
            }

            subject = contents.Subject;
            if (subject == null || subject.Length == 0)
            {
                throw new AuthenticationException("JWT with a missing subject");
            }

            foreach (var claim in contents.Claims)
            {
                if (claim.Type == USER_ROLE_CLAIM)
                {
                    continue;
                }
                claims[claim.Type] = claim;
            }

            if (!claims.ContainsKey(SERIAL_CLAIM))
            {
                throw new AuthenticationException("JWT with a missing serial token id");
            }
            serial = claims[SERIAL_CLAIM].Value;

            if (!claims.ContainsKey(USER_NAME_CLAIM))
            {
                throw new AuthenticationException("JWT with a missing user claim");
            }
            user = claims[USER_NAME_CLAIM].Value;
            
            foreach (var claim in contents.Claims)
            {
                if (claim.Type == USER_ROLE_CLAIM)
                {
                    roles.Add(claim.Value);
                }
            }

            if (roles.Count == 0)
            {
                throw new AuthenticationException("JWT with a missing role claims");
            }
        }

        public string Value
        {
            get { return tokenString; }
        }

        public override string ToString()
        {
            return Value;
        }

        public static JsonWebToken From(string tokenString)
        {
            return From(tokenString, JWT_ISSUER);
        }
        
        public static JsonWebToken From(string tokenString, string issuer)
        {
            return new JsonWebToken(tokenString, issuer);
        }

        public static JsonWebToken New(string signingKey, Guid userTokenId, string userName, string userId,
            List<string> roles, DateTime expiresAt, string issuer)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(USER_NAME_CLAIM, userName));
            claims.Add(new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, userId));
            claims.Add(new Claim(SERIAL_CLAIM, userTokenId.ToString()));
            foreach (var role in roles) 
            {
                claims.Add(new Claim(USER_ROLE_CLAIM, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey));
            var header = new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var issuedAt = DateTime.Now;
            var payload = new JwtPayload(issuer, null, claims, issuedAt, expiresAt, issuedAt);

            var token = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            return new JsonWebToken(handler.WriteToken(token), issuer);
        }

        public static JsonWebToken New(string signingKey, Guid userTokenId, string userName, string userId,
            List<string> roles, DateTime expiresAt)
        {
            return New(signingKey, userTokenId, userName, userId, roles, expiresAt, JWT_ISSUER);
        }

        public void AssertTokenIsValid(string signingKey)
        {
            try
            {
//                var token = new JwtSecurityToken(tokenString);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(tokenString, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidIssuer = jwtIssuer,
                    IssuerSigningKey = key
                }, out SecurityToken validatedToken);
            }
            catch (Exception ex)
            {
                throw new AuthenticationException("JWT failed validation");
            }
        }

        public void AssertTokenIsExpired()
        {
            if (IsExpired)
            {
                throw new AuthenticationException("JWT is expired");
            }
        }

        public bool IsExpired
        {
            get { return DateTime.Now >= expiresAt; }
        }

        public DateTime IssuedAt
        {
            get { return issuedAt; }
        }

        public DateTime ExpiresAt
        {
            get { return expiresAt; }
        }

        public string User
        {
            get { return user; }
        }

        public Guid UserId
        {
            get { return new Guid(subject); }
        }

        public string Subject
        {
            get { return subject; }
        }

        public string TokenId
        {
            get { return serial; }
        }

        public string Serial
        {
            get { return serial; }
        }

        public bool HasRole(string role)
        {
            if (role == null || role.Length == 0)
            {
                return false;
            }

            return roles.Contains(role);
        }

        public string[] GetRoles()
        {
            return roles.ToArray();
        }

        public string Issuer
        {
            get { return issuer; }
        }

        public Claim GetClaim(string claimKey)
        {
            return claims[claimKey];
        }

        public bool HasClaim(string claimKey)
        {
            return claims.ContainsKey(claimKey);
        }
    }
}