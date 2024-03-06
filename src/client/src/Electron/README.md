Renderer and main folders must be completely seperate (not share files) because electron's renderer and main processes run on different threads and cannot talk with one another.
