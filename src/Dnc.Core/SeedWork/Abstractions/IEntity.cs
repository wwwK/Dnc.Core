﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Dnc.Seedwork
{
    /// <summary>
    /// Interface for entity.
    /// </summary>
    public interface IEntity
    {
        #region Public props.
        DataStatusEnum DataStatus { get; set; }

        bool CanBeRemoved { get; }

        bool CanBeSaved { get; }
        #endregion
    }
    public interface IEntity<TEntityId>
        : IEntity
    {
        TEntityId Id { get; set; }
    }
}