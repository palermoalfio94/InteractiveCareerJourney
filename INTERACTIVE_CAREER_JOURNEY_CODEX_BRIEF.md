# Interactive Career Journey --- Project Brief for Codex

## 1. Project Overview

This project is an interactive personal portfolio presented as a short
2.5D narrative experience rather than a traditional CV website.

The visitor watches a stylized character representing Alfio Palermo move
along a road while his educational and professional life progressively
appears around him.

The experience should feel like an interactive animated story /
game-like portfolio, not like a conventional website and not like a
difficult videogame.

The visitor must be able to:

-   watch the career journey unfold automatically;
-   pause and resume the animation at any time;
-   click important objects, buildings and milestones;
-   inspect projects, education, jobs and other details;
-   zoom out and explore everything that has already appeared;
-   skip the narrative and directly explore the completed portfolio;
-   follow external links to projects, repositories, videos,
    publications, CV, etc.

Everything that appears during the journey should remain in the world.
At the end, the complete scene becomes a visual map of the entire
career.

------------------------------------------------------------------------

## 2. Core Concept

The main character starts at approximately age 13.

The camera follows him from a mostly lateral perspective in a stylized
2.5D environment.

As he walks toward the right, locations, characters, buildings, objects
and signs appear along the road.

Time advances rapidly. Years of real life can be represented in a few
seconds.

Example opening sequence:

1.  Young Alfio walks toward his middle school.
2.  A teacher/professor appears and introduces the idea of studying
    Computer Science.
3.  A short speech bubble appears:
    -   Teacher: "Would you like to study Computer Science?"
    -   Alfio: "Yes, why not?"
4.  Transition: "5 YEARS LATER".
5.  The character becomes older.
6.  Visual elements representing early skills appear:
    -   C++
    -   PHP
    -   HTML
    -   CSS
    -   database basics
7.  The first school programming project becomes clickable.
8.  A humorous caption may explain that the project is ugly, but it was
    made before generative AI could write it for him.

This opening should ideally take roughly 10--15 seconds.

The narrative then continues through university, software development,
game development, work, teaching, return to university, machine
learning, deep learning, computer vision, research and personal
projects.

------------------------------------------------------------------------

## 3. Narrative Principle

The world is the CV.

Do not treat the environment merely as decoration around traditional UI
panels.

Whenever possible, communicate career information spatially and
visually.

Examples:

-   University is represented by an actual university building.
-   Employment is represented by workplaces/companies.
-   Teaching is represented by a school.
-   Software/game development can be represented through computers,
    game-related props or an office.
-   AI/ML/CV work can progressively introduce more
    technological/research-oriented structures.
-   Personal and university projects can exist as clickable objects
    associated with the relevant location.

Parallel activities can be represented visually.

For example, if Alfio is studying and working simultaneously, the road
could split or the scene could visually communicate two concurrent
paths.

When university is temporarily paused, the university path should remain
physically present in the world rather than disappearing.

When studies resume later, the narrative can reconnect to that previous
path.

------------------------------------------------------------------------

## 4. Character Evolution

The protagonist represents Alfio at different ages.

The character does NOT need to be photorealistic.

Prefer a simple stylized low-poly character whose age and life phase can
be communicated through:

-   body proportions;
-   height;
-   clothing;
-   backpack;
-   laptop;
-   accessories;
-   context.

Possible phases:

-   age \~13;
-   technical high-school student;
-   university student;
-   software developer;
-   working professional;
-   teacher;
-   MSc / AI researcher phase.

The visual identity should remain recognizable across phases.

Avoid requiring custom character modeling if possible.

------------------------------------------------------------------------

## 5. Art Direction

Preferred direction:

-   stylized;
-   low-poly;
-   clean;
-   friendly;
-   professional enough for a technical portfolio;
-   lightweight enough for WebGL;
-   visually coherent;
-   somewhat like a miniature diorama.

Avoid:

-   photorealism;
-   highly detailed models;
-   manually modeling dozens of unique structures;
-   inconsistent asset packs;
-   generic "student Unity project" appearance.

Asset reuse is strongly encouraged.

Priority should be:

1.  Free existing assets.
2.  Modular asset packs.
3.  Procedural/custom assembly using existing assets.
4.  AI-generated assets where appropriate.
5.  Manual Blender modeling only as a last resort.

Potential asset ecosystems already considered:

-   Quaternius;
-   Kenney;
-   Mixamo for humanoid animations;
-   Poly Pizza for additional props/models.

Prefer a small number of visually compatible asset families rather than
many unrelated free packs.

------------------------------------------------------------------------

## 6. Technical Direction

Primary engine:

**Unity**

Target:

**WebGL**, embedded in or used as the main experience of a personal
portfolio website.

Visual structure:

**2.5D**

The world can be fully 3D internally, but movement and storytelling
should primarily follow a horizontal journey.

The camera should generally follow the protagonist while retaining
cinematic freedom.

Potential Unity technologies:

-   Cinemachine for camera behaviour;
-   DOTween or an equivalent tweening system for procedural animation;
-   Unity Animator for character animation;
-   Timeline only where useful;
-   Unity UI / UI Toolkit depending on implementation needs;
-   ScriptableObjects or data-driven JSON for narrative content.

Avoid hardcoding the entire biography directly into scene logic.

------------------------------------------------------------------------

## 7. Data-Driven Journey System

The project should be designed so that new career events can be added
without rewriting core systems.

Conceptually, an event might contain data such as:

-   year/date;
-   title;
-   description;
-   character age/variant;
-   destination/location;
-   character action;
-   dialogue;
-   structure to spawn;
-   spawn animation;
-   camera behaviour;
-   duration;
-   clickable content;
-   associated projects;
-   external links;
-   media;
-   skills gained.

Example conceptual structure:

``` text
JourneyEvent

Year: 2013
Title: University
Character: Alfio_University
Location: University
Action: WalkTo
Duration: 4.0

Dialogue:
"Computer Science, here we go."

Spawn:
UniversityBuilding

SpawnAnimation:
BuildFromGround

Camera:
FollowThenFocus

Clickable:
true

Content:
UniversityProfile
```

Exact implementation is open to architectural improvement.

------------------------------------------------------------------------

## 8. Persistent World

This is a critical requirement.

Objects introduced during the journey should remain available after
their narrative moment.

Example:

At time T1: - school appears.

At T2: - university appears.

At T3: - workplace appears.

When T3 is reached, school and university must still exist behind the
protagonist.

The final result should be a long visual timeline/world representing the
complete career.

The user should eventually be able to zoom out and see the entire
journey.

------------------------------------------------------------------------

## 9. Interactive Elements

Important world objects should support hover/click interaction.

Possible interactions:

-   hover outline/highlight;
-   small label;
-   click to focus;
-   camera transition toward the object;
-   pause narrative;
-   open detailed information;
-   display media;
-   open project links;
-   return to journey.

Examples of clickable elements:

-   schools;
-   university;
-   companies;
-   projects;
-   computers;
-   game-development milestones;
-   research projects;
-   publications;
-   personal projects.

Interaction must never require completing challenges.

This is a portfolio, not a skill test for the visitor.

------------------------------------------------------------------------

## 10. Narrative Playback

Default experience:

**Play Journey**

The character automatically moves through the story.

Controls should include at minimum:

-   pause;
-   resume;
-   skip / explore;
-   possibly playback speed;
-   sound toggle.

Clicking an important element during playback should pause the journey
automatically.

After closing the detail view, the visitor should be able to resume from
the same point.

------------------------------------------------------------------------

## 11. Explore Mode

A second major mode is:

**Explore**

This can be entered:

-   after the story finishes;
-   through a Skip/Explore button;
-   possibly at any moment.

In Explore mode:

-   automatic narrative movement stops;
-   camera navigation becomes more flexible;
-   visitor can pan/drag through the world;
-   visitor can zoom in/out;
-   all unlocked/available portfolio objects are clickable.

At the end of the full journey, zooming out should reveal the entire
career as one large 2.5D diorama.

This final overview is an important visual payoff.

------------------------------------------------------------------------

## 12. Animation Philosophy

Animation quality matters significantly.

However, avoid manually authoring unique animations for every object.

Prefer reusable procedural animation systems.

Examples:

### Building appearance

Buildings can:

-   rise from the ground;
-   assemble from modular pieces;
-   scale from 0 to 1 with controlled easing;
-   have components appear sequentially;
-   settle with a subtle bounce;
-   activate windows/signs after construction.

### Props

Props can:

-   pop in;
-   slide into position;
-   rotate into place;
-   unfold;
-   fade/scale.

### Camera

Camera transitions should feel cinematic but short.

Possible sequence when clicking a building:

1.  Narrative pauses.
2.  Camera stops following character.
3.  Camera moves toward building.
4.  Slight orbit/reframing.
5.  Detail UI appears.
6.  On exit, camera returns.
7.  Narrative can resume.

Animation should support fast pacing. The visitor should never wait
several seconds unnecessarily just to access information.

------------------------------------------------------------------------

## 13. Reusable Animation System

Build generic components rather than custom animation scripts for every
scene.

Potential concepts:

-   `SpawnAnimator`
-   `BuildingAnimator`
-   `CharacterJourneyController`
-   `JourneyDirector`
-   `CameraDirector`
-   `InteractivePOI`
-   `PortfolioContent`
-   `DialogueController`
-   `ExploreModeController`

Names are suggestions, not mandatory architecture.

The important requirement is modularity.

------------------------------------------------------------------------

## 14. Character Animation

Do not manually animate humanoid characters unless necessary.

Prefer existing animation libraries such as Mixamo.

Likely required animation set:

-   Idle
-   Walk
-   Talk
-   Point
-   Think
-   Sit
-   Type
-   Celebrate
-   Look Around

Use retargetable humanoid animations where possible.

Codex should help configure Animator Controllers, transitions and
narrative triggers.

------------------------------------------------------------------------

## 15. UI / Dialogue

Dialogue should be short.

Use speech bubbles or lightweight contextual captions.

Avoid large blocks of text during automatic playback.

Example:

``` text
Teacher:
"Have you ever thought about studying Computer Science?"

Alfio:
"Why not?"
```

Detailed information belongs in clickable portfolio panels, not in the
main narrative.

The main story should remain fast and enjoyable.

------------------------------------------------------------------------

## 16. Portfolio Detail Panels

When a POI is selected, a detail view can show:

-   title;
-   date/year;
-   short description;
-   technologies;
-   skills;
-   screenshots;
-   video;
-   project links;
-   GitHub;
-   publications;
-   downloadable CV where appropriate.

The detailed interface can be more conventional than the narrative
itself.

Usability is more important than gamification here.

------------------------------------------------------------------------

## 17. Web Portfolio Requirements

This project is intended for recruiters, researchers, technical
professionals and other visitors who may have limited time.

Therefore:

-   loading time matters;
-   WebGL build size matters;
-   mobile/fallback behaviour should eventually be considered;
-   no mandatory gameplay;
-   no difficult controls;
-   information must be quickly accessible;
-   "Skip to Portfolio / Explore" should always be easy to find;
-   external links must be easy to open;
-   the experience should not hide professional information behind
    puzzles.

The interactive presentation should improve the portfolio, never make it
harder to use.

------------------------------------------------------------------------

## 18. Performance

Because the target is WebGL:

-   prefer low-poly models;
-   reuse materials;
-   use texture atlases where useful;
-   avoid excessive real-time lights;
-   use baked lighting where appropriate;
-   pool objects where useful;
-   minimize draw calls;
-   avoid unnecessarily high-resolution textures;
-   keep animation systems lightweight;
-   avoid loading every heavy media asset immediately.

The full career world may eventually contain many objects, so
scalability should be considered early.

------------------------------------------------------------------------

## 19. Initial Prototype / Vertical Slice

Do NOT build the complete biography first.

The first milestone should be a polished vertical slice representing
approximately the first 10--15 seconds.

Prototype sequence:

1.  Empty/minimal road.
2.  13-year-old Alfio enters and walks.
3.  Middle-school environment appears.
4.  Teacher appears.
5.  Teacher and Alfio stop.
6.  Short dialogue about studying Computer Science.
7.  Alfio continues walking.
8.  "5 YEARS LATER" transition.
9.  Character switches/grows to older version.
10. C++, PHP, HTML/CSS and database-related visual elements appear.
11. First school project appears as clickable POI.
12. Clicking it pauses the narrative and opens a simple project panel.
13. Closing the panel returns to the journey.
14. User can zoom out enough to see the portion of world already
    constructed.

This vertical slice should validate:

-   art style;
-   character style;
-   asset compatibility;
-   camera;
-   narrative pacing;
-   dialogue;
-   spawn animations;
-   interaction;
-   WebGL performance.

Only after this works and looks good should the full timeline be
implemented.

------------------------------------------------------------------------

## 20. Development Philosophy

The project should be developed primarily through coding-agent
assistance.

The human role is closer to:

**creative director + product owner + tester**

rather than manually writing every script.

Codex should proactively:

-   create and modify Unity C# scripts;
-   propose architecture;
-   create reusable systems;
-   automate repetitive Editor work;
-   create Unity Editor tools where useful;
-   write tests;
-   inspect errors/logs when accessible;
-   refactor systems;
-   automate asset setup where practical;
-   keep project documentation updated.

Avoid requiring repetitive manual Unity Editor operations when an Editor
script or reusable tool can perform them safely.

However, do not automate for its own sake. Simple Editor actions can
remain manual when automation would be more complex than the task
itself.

------------------------------------------------------------------------

## 21. Artistic Constraint

The owner does NOT want to spend significant time manually modeling
structures in Blender.

Treat this as a project constraint.

When an asset is needed, prefer:

1.  existing compatible asset;
2.  assembling modular existing assets;
3.  procedural generation;
4.  AI-assisted generation;
5.  simple custom geometry generated through Unity/Blender scripts;
6.  manual modeling only when unavoidable.

------------------------------------------------------------------------

## 22. Tone

The portfolio should communicate technical competence without taking
itself too seriously.

Small humorous moments are encouraged.

Example:

First programming project:

> "It's not pretty. In my defense, generative AI wasn't around to write
> it for me."

Humour should remain subtle and professional enough that the site can be
shared with:

-   recruiters;
-   professors;
-   PhD committees;
-   researchers;
-   companies.

------------------------------------------------------------------------

## 23. Current Story Outline

The precise dates/content will be refined later.

Current high-level narrative:

``` text
Age ~13
    ↓
First exposure to Computer Science
    ↓
Technical high school
    ↓
C++ / PHP / HTML / CSS / databases
    ↓
First programming projects
    ↓
Computer Science university
    ↓
University projects and coursework
    ↓
Work begins while studying
    ↓
Software / game development
    ↓
BSc graduation
    ↓
Professional development
    ↓
Company change
    ↓
University temporarily paused
    ↓
Teaching career
    ↓
Return to university — MSc Computer Science
    ↓
Machine Learning
    ↓
Deep Learning
    ↓
Computer Vision
    ↓
Synthetic data / VQA / AI projects
    ↓
Research / future direction
```

Parallel events should be visually represented where appropriate rather
than forcing everything into a strictly linear sequence.

------------------------------------------------------------------------

## 24. Future Extensibility

The portfolio should remain easy to update for years.

Future events may include:

-   MSc graduation;
-   new publications;
-   PhD;
-   research projects;
-   new jobs;
-   startups;
-   open-source projects.

Adding a new milestone should ideally mean creating new event data and
assigning existing reusable assets/behaviours rather than modifying the
core engine.

------------------------------------------------------------------------

## 25. Immediate Goal

The immediate goal is NOT to implement the entire portfolio.

First:

1.  establish the Unity project structure;
2.  evaluate/select the visual asset family;
3.  implement the journey/event architecture;
4.  implement character movement;
5.  implement reusable spawn animation;
6.  implement camera follow/focus;
7.  implement dialogue;
8.  implement clickable POIs;
9.  implement pause/resume;
10. build the opening 10--15 second vertical slice;
11. test as WebGL;
12. evaluate whether the artistic result is strong enough before
    expanding.

When making architectural decisions, optimize for:

**visual quality + maintainability + rapid iteration + WebGL
performance + minimal manual asset work.**
