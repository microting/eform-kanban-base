namespace Microting.KanbanBase.Infrastructure.Enums;

/// <summary>A member's role in a workspace. Lower is more: Admin manages members and keys, Team works the boards,
/// Customer sees only its own cases (spec 2026-10-30 §4).</summary>
public enum WorkspaceRole
{
    Admin = 1,
    Team = 2,
    Customer = 3,
}
