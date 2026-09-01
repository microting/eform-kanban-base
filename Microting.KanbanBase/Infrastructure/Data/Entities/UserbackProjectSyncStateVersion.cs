using System;
using Microting.eFormApi.BasePn.Infrastructure.Database.Base;

namespace Microting.KanbanBase.Infrastructure.Data.Entities;

public class UserbackProjectSyncStateVersion : BaseEntity
{
    public int UserbackProjectSyncStateId { get; set; }
    public int UserbackProjectId { get; set; }
    public DateTime? LastSyncedModifiedAt { get; set; }
}
