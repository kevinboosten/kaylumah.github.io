﻿// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System.Threading.Tasks;
using Kaylumah.Ssg.Access.Artifact.Interface;

namespace Kaylumah.Ssg.Access.Artifact.Service;

public interface IStoreArtifactsStrategy
{
    Task Execute(StoreArtifactsRequest request);
    bool ShouldExecute(StoreArtifactsRequest request);
}