using System.Security;
#if NET9_0_OR_GREATER
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(System.Threading.Lock))]
#endif
[assembly: SecurityTransparent()]