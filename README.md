Monadic Exploration w/.NET Editor and Site Expansion
===================

Brought to my attention by members of the College Conference of Composition and Communication, this handy visualization has great uses; however, for those not seasoned with JSON formatting, the data file can be beastly to maintain for large sets of data.  I'm attempting to skin the top of this with a very basic user editing platform and database backing to support an entire site of monadic explorations with editing capabilities.  The plan is, in theory, keep it as simple as possible so others can fork it into other languages and expand on the editing concepts and functionality.  Emphasis will be more on converting the data.json into a database and the functionality of editing it.  Minimal focus will be put user entry error and validation as that's a subjective area and everyone has their own flavor.  - Pete

Other project references:
[.NET Json serialization via Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
[Entity Framework v6.1.3](https://msdn.microsoft.com/en-us/data/ef.aspx)

SPECIAL NOTE: Yes... there are areas redundant code can be reduced.  Again, emphasis is to get a working codebase out there for everyone else to do with as they please.

**Marian Dörk's Original README follows:**

Monadic Exploration
===================

Monadic exploration is a new approach to interacting with relational information spaces that challenges the distinction between the whole and its parts. Building on the work of sociologists Gabriel Tarde and Bruno Latour we turn to the concept of the monad as a useful lens on online communities and collections that expands the possibility for creating meaning in their navigation. While existing interfaces tend to emphasize either the structure of the whole or details of a part, monadic exploration brings these opposing perspectives closer together in continuous movements between partially overlapping points of view. The resulting visualization reflects a given node’s relative position within a network using radial displacements and visual folding.

See also the [project page](http://mariandoerk.de/monadicexploration/) and [demo](http://mariandoerk.de/monadicexploration/demo/).

We engaged in an iterative design process together with the makers of [Beautiful Trouble](http://beautifultrouble.org/), a highly cross-referenced book on creative forms of activism.

![Loop GIF](http://mariandoerk.de/monadicexploration/loop.gif)
