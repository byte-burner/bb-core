# add more command handlers in this folder

Rules: 
1. Handlers can call other handlers
2. Dependency injection doesn't work in handler clasess because invoke is used to call them, which creates a blank class from outside our host 