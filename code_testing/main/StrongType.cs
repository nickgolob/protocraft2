namespace main;

// T should be primative. Non-primatives can inheret directly.
// https://www.google.com/search?q=c%23+int+alias+strong+type+stack+overflow&sca_esv=891f019e1683b014&sxsrf=ACQVn09tW9pn4T87tx3oiHB-rj3olXyfuw%3A1707257045496&ei=1azCZYT0Hcbk0PEPuam98As&oq=c%23+int+alias+strong+type+stack&gs_lp=Egxnd3Mtd2l6LXNlcnAiHmMjIGludCBhbGlhcyBzdHJvbmcgdHlwZSBzdGFjayoCCAAyBRAhGKABSLExUL0HWN4mcAF4AZABAJgBrgGgAZsSqgEEMjMuM7gBA8gBAPgBAcICChAAGEcY1gQYsAPCAgoQLhiABBiKBRhDwgIREAAYgAQYigUYkQIYsQMYgwHCAhAQABiABBiKBRhDGLEDGIMBwgIKEAAYgAQYigUYQ8ICGRAuGIAEGIoFGEMYlwUY3AQY3gQY4ATYAQHCAgoQABiABBgUGIcCwgIFEAAYgATCAgsQABiABBiKBRiRAsICBhAAGBYYHsICBRAhGKsCwgIFECEYnwXiAwQYACBBiAYBkAYIugYGCAEQARgU&sclient=gws-wiz-serp
// https://stackoverflow.com/questions/850306/c-sharp-deriving-from-int32
public struct StrongType<TPrimative>(TPrimative value_) {
    private TPrimative value = value_;

    public static implicit operator StrongType<TPrimative>(TPrimative value) {
        return new StrongType<TPrimative>(value);
    }
}

public class Test {
    public TestMethod() {
        X(5);
        Y(5);
    }

    public void X(int y) { }
    public void Y(StrongType<int> y) { }
}