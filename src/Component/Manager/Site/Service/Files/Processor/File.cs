﻿// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;
using Kaylumah.Ssg.Manager.Site.Service.Files.Metadata;

namespace Kaylumah.Ssg.Manager.Site.Service.Files.Processor;

[DebuggerDisplay("File (Name={Name})")]
public class File
{
    public DateTimeOffset LastModified { get; set; }
    public FileMetaData MetaData { get; set; }
    public string Content { get; set; }
    public string Name { get; set; }
}