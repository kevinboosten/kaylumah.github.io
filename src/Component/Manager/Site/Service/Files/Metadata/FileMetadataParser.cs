﻿// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ssg.Extensions.Metadata.Abstractions;

namespace Kaylumah.Ssg.Manager.Site.Service.Files.Metadata;

public class FileMetadataParser : IFileMetadataParser
{
    private readonly ILogger _logger;
    private readonly IMetadataProvider _metadataProvider;
    private readonly MetadataParserOptions _options;
    public FileMetadataParser(ILogger<FileMetadataParser> logger, IMetadataProvider metadataProvider, IOptions<MetadataParserOptions> options)
    {
        _logger = logger;
        _metadataProvider = metadataProvider;
        _options = options.Value;
    }

    public Metadata<FileMetaData> Parse(MetadataCriteria criteria)
    {
        var result = _metadataProvider.Retrieve<FileMetaData>(criteria.Content);

        var outputLocation = DetermineOutputLocation(criteria.FileName, criteria.Permalink, result.Data);
        var paths = DetermineFilters(outputLocation);

        var fileMetaData = ApplyDefaults(paths);
        OverwriteMetaData(fileMetaData, result.Data, "file");

        if (fileMetaData.Date != null && string.IsNullOrEmpty(fileMetaData.PublishedDate))
        {
            fileMetaData.PublishedDate = fileMetaData.Date.GetValueOrDefault().ToString("yyyy-MM-dd");
        }

        if (!string.IsNullOrEmpty(fileMetaData.PublishedDate) && !string.IsNullOrEmpty(fileMetaData.PublishedTime))
        {
            var publishedDateTimeString = $"{fileMetaData.PublishedDate} {fileMetaData.PublishedTime}";
            var publishedDate = System.DateTimeOffset.ParseExact(publishedDateTimeString, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            fileMetaData.Date = publishedDate;
        }

        if (!string.IsNullOrEmpty(fileMetaData.PublishedDate) && string.IsNullOrEmpty(fileMetaData.ModifiedDate))
        {
            fileMetaData.ModifiedDate = fileMetaData.PublishedDate;
        }
        if (!string.IsNullOrEmpty(fileMetaData.PublishedTime) && string.IsNullOrEmpty(fileMetaData.ModifiedTime))
        {
            fileMetaData.ModifiedTime = fileMetaData.PublishedTime;
        }

        if (!string.IsNullOrEmpty(fileMetaData.ModifiedDate))
        {
            var dateTimeString = !string.IsNullOrEmpty(fileMetaData.ModifiedTime) ? $"{fileMetaData.ModifiedDate} {fileMetaData.ModifiedTime}" : fileMetaData.ModifiedDate;
            var dateTimePattern = !string.IsNullOrEmpty(fileMetaData.ModifiedTime) ? "yyyy-MM-dd HH:mm" : "yyyy-MM-dd";
            var modifiedDateTime = System.DateTimeOffset.ParseExact(dateTimeString, dateTimePattern, System.Globalization.CultureInfo.InvariantCulture);
            fileMetaData.Modified = modifiedDateTime;
        }

        // we now have applied all the defaults that match this document and combined it with the retrieved data, store it.
        result.Data = fileMetaData;

        // TODO: is this the right moment to store it back in the metadata?
        result.Data.Uri = outputLocation;

        return result;
    }

    private string RetrieveExtension(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        if (_options.ExtensionMapping.ContainsKey(ext))
        {
            return _options.ExtensionMapping[ext];
        }
        return ext;
    }

    private FileMetaData ApplyDefaults(List<string> filters)
    {
        var fileMetaData = new FileMetaData();
        foreach (var filter in filters)
        {
            var meta = _options.Defaults.SingleOrDefault(x => x.Path.Equals(filter));
            if (meta != null)
            {
                OverwriteMetaData(fileMetaData, meta.Values, $"default:{filter}");
            }
        }
        return fileMetaData;
    }

    private List<string> DetermineFilters(string outputLocation)
    {
        var paths = new List<string>() { string.Empty };
        var index = outputLocation.LastIndexOf(Path.DirectorySeparatorChar);
        if (index >= 0)
        {
            var input = outputLocation.Substring(0, index);
            paths.AddRange(DetermineFilterDirectories(input));
            paths = paths.OrderBy(x => x.Length).ToList();
        }
        return paths;
    }

    private List<string> DetermineFilterDirectories(string input)
    {
        var result = new List<string>();
        int index;
        while ((index = input.LastIndexOf(Path.DirectorySeparatorChar)) >= 0)
        {
            result.Add(input);
            input = input.Substring(0, index);
        }

        if (!string.IsNullOrEmpty(input))
        {
            result.Add(input);
        }
        return result;
    }

    private string DetermineOutputLocation(string fileName, string permalink, FileMetaData metaData)
    {
        var pattern = @"((?<year>\d{4})\-(?<month>\d{2})\-(?<day>\d{2})\-)?(?<filename>[\s\S]*?)\.(?<ext>.*)";
        var match = Regex.Match(fileName, pattern);

        var outputFileName = match.FileNameByPattern();
        var fileDate = match.DateByPattern();
        if (fileDate != null)
        {
            metaData.Date = fileDate;
        }

        var outputExtension = RetrieveExtension(outputFileName);

        var result = permalink
            .Replace("/:year", fileDate == null ? string.Empty : $"/{fileDate?.ToString("yyyy")}")
            .Replace("/:month", fileDate == null ? string.Empty : $"/{fileDate?.ToString("MM")}")
            .Replace("/:day", fileDate == null ? string.Empty : $"/{fileDate?.ToString("dd")}");

        result = result.Replace(":name", Path.GetFileNameWithoutExtension(outputFileName))
            .Replace(":ext", outputExtension);

        if (result.StartsWith("/"))
        {
            result = result[1..];
        }
        return result;
        //metaData.Uri = result;
        //metaData.Remove(nameof(metaData.Permalink).ToLower());
    }

    private void OverwriteMetaData(FileMetaData target, FileMetaData source, string reason)
    {
        if (source != null)
        {
            foreach (var entry in source)
            {
                if (target.ContainsKey(entry.Key))
                {
                    _logger.LogInformation($"Overwritting '{entry.Key}' with '{entry.Value}' instead of {target[entry.Key]} because '{reason}'");

                }
                target[entry.Key] = entry.Value;
            }
        }
    }

}