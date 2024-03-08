namespace main;

// IN PROGRESS -- PROBABLY NOT. (cant inheret struct?)

// T should be primative. Non-primatives can inheret directly.
// https://www.google.com/search?q=c%23+int+alias+strong+type+stack+overflow&sca_esv=891f019e1683b014&sxsrf=ACQVn09tW9pn4T87tx3oiHB-rj3olXyfuw%3A1707257045496&ei=1azCZYT0Hcbk0PEPuam98As&oq=c%23+int+alias+strong+type+stack&gs_lp=Egxnd3Mtd2l6LXNlcnAiHmMjIGludCBhbGlhcyBzdHJvbmcgdHlwZSBzdGFjayoCCAAyBRAhGKABSLExUL0HWN4mcAF4AZABAJgBrgGgAZsSqgEEMjMuM7gBA8gBAPgBAcICChAAGEcY1gQYsAPCAgoQLhiABBiKBRhDwgIREAAYgAQYigUYkQIYsQMYgwHCAhAQABiABBiKBRhDGLEDGIMBwgIKEAAYgAQYigUYQ8ICGRAuGIAEGIoFGEMYlwUY3AQY3gQY4ATYAQHCAgoQABiABBgUGIcCwgIFEAAYgATCAgsQABiABBiKBRiRAsICBhAAGBYYHsICBRAhGKsCwgIFECEYnwXiAwQYACBBiAYBkAYIugYGCAEQARgU&sclient=gws-wiz-serp
// https://stackoverflow.com/questions/850306/c-sharp-deriving-from-int32
// https://stackoverflow.com/questions/12956567/bidirectional-implicit-casting
public struct StrongType<TPrimitive>(TPrimitive value_) {
  private TPrimitive value { get; } = value_;

  // Tprimitive -> StrongType
  public static implicit operator StrongType<TPrimitive>(TPrimitive input) {
    return new StrongType<TPrimitive>(input);
  }

  public static implicit operator TPrimitive(StrongType<TPrimitive> input) {
    return input.value;
  }
}

// // arms length = 3 ft
// public struct DistanceFeet : StrongType<float> { }
//
// // short sword blade = 5 lbs. Battle axe weight = 20 lbs?
// using WeightPounds = double;
//
// // wood == 1.0. Steel = 2.0?
// using Hardness = double;