// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Security;
#if NET9_0_OR_GREATER
using System.Runtime.CompilerServices;

[assembly: TypeForwardedTo(typeof(System.Threading.Lock))]
#endif

#if !SOURCE_GENERATOR
[assembly: SecurityTransparent]
#endif
