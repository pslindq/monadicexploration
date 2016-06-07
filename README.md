Monadic Exploration w/.NET Editor and Site Expansion
===================

Brought to my attention by members of the College Conference of Composition and Communication, this handy visualization has great uses; however, for those not seasoned with JSON formatting, the data file can be beastly to maintain for large sets of data.  I attempted to skin the top of this with a very basic user editing platform and database backing to support an entire site of monadic explorations via extensionless URL rewriting with editing capabilities.  The plan was, in theory, keep it as simple as possible so others can fork it into other languages and expand on the editing concepts and functionality.  Emphasis was more on converting the data.json into a database schema and the functionality of editing it.  Minimal focus was put user entry error, validation, and editing UX as that's a subjective area and everyone has their own flavor.

Basic Operations
--------------------
If you want to view the original project files, I retained them and they are accessible from the root of the project at */index.html*.  For the editable version (and forgive me if I leave testing data in the database) that supports password-enabled editing access and multiple monad visualizations based on URL segments, start your journey from */default.aspx*.

At this root page, you can select from all known visualizations stored in the database or create a new one.  To create a new one, provide the "master password" as defined in the */web.config* file and follow the prompted directions on the form.  If you select one of the existing visualizations, use the link that appears in the upper right of the visualization page to enter edit mode.  **Note** - to edit in individual visualization you need either the master password or the monad-specific password as defined in the database (currently stored in the clear).  Again, with editing, follow the prompted directions as they are fairly self-explanatory.

Other Project References
--------------------
[.NET Json serialization via Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)  
[Entity Framework v6.1.3](https://msdn.microsoft.com/en-us/data/ef.aspx)

**SPECIAL NOTE:** Yes... there are areas redundant code can be reduced, and it's not that secure.  Again, emphasis was to get a working codebase out there for everyone else to do with as they please. Deuces. 

**Marian Dörk's Original README follows:**

Monadic Exploration
===================

Monadic exploration is a new approach to interacting with relational information spaces that challenges the distinction between the whole and its parts. Building on the work of sociologists Gabriel Tarde and Bruno Latour we turn to the concept of the monad as a useful lens on online communities and collections that expands the possibility for creating meaning in their navigation. While existing interfaces tend to emphasize either the structure of the whole or details of a part, monadic exploration brings these opposing perspectives closer together in continuous movements between partially overlapping points of view. The resulting visualization reflects a given node’s relative position within a network using radial displacements and visual folding.

See also the [project page](http://mariandoerk.de/monadicexploration/) and [demo](http://mariandoerk.de/monadicexploration/demo/).

We engaged in an iterative design process together with the makers of [Beautiful Trouble](http://beautifultrouble.org/), a highly cross-referenced book on creative forms of activism.

![Loop GIF](http://mariandoerk.de/monadicexploration/loop.gif)
