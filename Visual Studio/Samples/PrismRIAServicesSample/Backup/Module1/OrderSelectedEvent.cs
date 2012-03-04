///////////////////////////////////////////////////////////////////////////////
//
// Copyright (C) 2008-2009 David Hill. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
// KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
///////////////////////////////////////////////////////////////////////////////
using System;

using Microsoft.Practices.Composite.Presentation.Events;

using Prism.Samples.Web.OrderServices.DataModel;

namespace Prism.Samples.Module1
{
    public class OrderSelectedEvent : CompositePresentationEvent<Order>
    {
    }
}
