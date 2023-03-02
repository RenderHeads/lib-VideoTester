using System;
namespace LibVideoTester.Models {
  /// <summary>
  /// Possible reasons that our resolution check fails to give some insight to users.
  /// </summary>
  [System.Flags]
  public enum ResolutionValidationFailureReason {
    None = 0,
    NotDivisbleByFourWidth = 1,
    NotDivisbleByFourHeight = 2,
    WidthTooLarge = 4,
    HeightTooLarge = 8
  }

  public static class ResolutionValidationFailureReasonExtensions {
    public static ResolutionValidationFailureReason AppendIfFalse(
        this ResolutionValidationFailureReason current,
        ResolutionValidationFailureReason toAppend,
        bool isTrue) {
      if (isTrue) {
        return current;
      } else {
        return current == ResolutionValidationFailureReason.None ? toAppend : current | toAppend;
      }
    }
  }

}
