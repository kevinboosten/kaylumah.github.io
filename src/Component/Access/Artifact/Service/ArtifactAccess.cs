﻿// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kaylumah.Ssg.Access.Artifact.Interface;
using Microsoft.Extensions.Logging;

namespace Kaylumah.Ssg.Access.Artifact.Service;

public class ArtifactAccess : IArtifactAccess
{

    private readonly ILogger _logger;
    private readonly IEnumerable<IStoreArtifactsStrategy> _storeArtifactsStrategies;

    public ArtifactAccess(ILogger<ArtifactAccess> logger, IEnumerable<IStoreArtifactsStrategy> storeArtifactsStrategies)
    {
        _logger = logger;
        _storeArtifactsStrategies = storeArtifactsStrategies;
    }

    public async Task Store(StoreArtifactsRequest request)
    {
        var storeArtifactsStrategy = _storeArtifactsStrategies.SingleOrDefault(strategy => strategy.ShouldExecute(request));
        await storeArtifactsStrategy.Execute(request);
    }
}