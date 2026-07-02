using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum EstimatedJumpClass
{
	[Description(Labels.ClassJump2)] JumpClass2 = 2,
	[Description(Labels.ClassJump3)] JumpClass3 = 3,
	[Description(Labels.ClassJump4)] JumpClass4 = 4,
	[Description(Labels.ClassJump5)] JumpClass5 = 5,
	[Description(Labels.ClassJump6)] JumpClass6 = 6
}