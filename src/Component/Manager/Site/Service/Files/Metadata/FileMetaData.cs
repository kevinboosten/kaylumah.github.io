﻿// Copyright (c) Kaylumah, 2021. All rights reserved.
// See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kaylumah.Ssg.Manager.Site.Service.Files.Metadata;

public class FileMetaData : Dictionary<string, object>
{

    public string Series
    {
        get
        {
            return this.GetValue<string>(nameof(Series));
        }
        set
        {
            this.SetValue(nameof(Series), value);
        }
    }

    public string Layout
    {
        get
        {
            return this.GetValue<string>(nameof(Layout));
        }
        set
        {
            this.SetValue(nameof(Layout), value);
        }
    }

    public string Permalink
    {
        get
        {
            return this.GetValue<string>(nameof(Permalink));
        }
        set
        {
            this.SetValue(nameof(Permalink), value);
        }
    }

    public string Uri
    {
        get
        {
            return this.GetValue<string>(nameof(Uri));
        }
        set
        {
            this.SetValue(nameof(Uri), value);
        }
    }

    public string Collection
    {
        get
        {
            return this.GetValue<string>(nameof(Collection));
        }
        set
        {
            this.SetValue(nameof(Collection), value);
        }
    }

    public List<string> Tags
    {
        get
        {
            return this.GetValue<List<object>>(nameof(Tags))?.Cast<string>().ToList();
        }
        set
        {
            this.SetValue(nameof(Tags), value);
        }
    }

    public DateTimeOffset? Date
    {
        get
        {
            return this.GetValue<DateTimeOffset?>(nameof(Date));
        }
        set
        {
            this.SetValue(nameof(Date), value);
        }
    }

    public string PublishedDate
    {
        get
        {
            return this.GetValue<string>(nameof(PublishedDate));
        }
        set
        {
            this.SetValue(nameof(PublishedDate), value);
        }
    }

    public string PublishedTime
    {
        get
        {
            return this.GetValue<string>(nameof(PublishedTime));
        }
        set
        {
            this.SetValue(nameof(PublishedTime), value);
        }
    }

    public string ModifiedDate
    {
        get
        {
            return this.GetValue<string>(nameof(ModifiedDate));
        }
        set
        {
            this.SetValue(nameof(ModifiedDate), value);
        }
    }

    public string ModifiedTime
    {
        get
        {
            return this.GetValue<string>(nameof(ModifiedTime));
        }
        set
        {
            this.SetValue(nameof(ModifiedTime), value);
        }
    }

    public DateTimeOffset? Modified
    {
        get
        {
            return this.GetValue<DateTimeOffset?>(nameof(Modified));
            /*
            var txt = this.GetValue<string>(nameof(Modified));
            DateTimeOffset result;
            if (txt != null)
            {
                result = DateTimeOffset.ParseExact(txt, "yyyy-MM-dd HH:mm", null);
                this.SetValue(nameof(Modified), result);
                return result;
            } 
            else
            {
                return this.GetValue<DateTime?>(nameof(Modified));
            }
            */
        }
        set
        {
            this.SetValue(nameof(Modified), value);
        }
    }
}