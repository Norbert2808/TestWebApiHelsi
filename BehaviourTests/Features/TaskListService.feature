Feature: TaskListService
	TaskListService behaviour tests
	
Scenario: Create TaskList success
	Given there is AddTaskListCommand with name 'TestTaskList'
		And repository CreateAsync mock
	When ITaskListService.CreateAsync method executed
	Then this method should return TaskList with name 'TestTaskList'
	
Scenario: Update TaskList success
	Given there is UpdateTaskListCommand with name 'UpdateTestTaskList'
		And user has permission
		And repository GetByIdAsync mock
		And repository UpdateAsync mock
	When ITaskListService.UpdateAsync method executed
	Then this method should return TaskList with name 'UpdateTestTaskList'
	
Scenario: Update TaskList user doesn't have permission
	Given there is UpdateTaskListCommand with name 'UpdateTestTaskList'
		And user doesn't have permission
		And repository GetByIdAsync mock
	When ITaskListService.UpdateAsync method executed
	Then this method should return null
	
Scenario: Delete TaskList success
	Given there is DeleteTaskListCommand
		And user has permission
		And repository GetByIdAsync mock
		And repository DeleteAsync mock
	When ITaskListService.DeleteAsync method executed
	Then TaskList should be successfully deleted
	
Scenario: Delete TaskList user doesn't have permission
	Given there is DeleteTaskListCommand
		And user doesn't have permission
		And repository GetByIdAsync mock
	When ITaskListService.DeleteAsync method executed
	Then TaskList should not be deleted
	
Scenario: GetById TaskList success
	Given there is GetByIdTaskListCommand with id 7
		And user has permission
		And repository GetByIdAsync mock
	When ITaskListService.GetByIdAsync method executed
	Then this method should return TaskListFullModel with id 7
	
Scenario: GetById TaskList user doesn't have permission
	Given there is GetByIdTaskListCommand with id 3
		And user doesn't have permission
		And repository GetByIdAsync mock
	When ITaskListService.GetByIdAsync method executed
	Then this method should return null
	
Scenario: GetAll TaskList success itemsPerPage 3 and page 3
	Given there is GetAllTaskListCommand with itemsPerPage 3 and page 3
		And user has permission for 11 TaskLists
	When ITaskListService.GetAllAsync method executed
	Then this method should return collection with 3 elements
	
Scenario: GetAll TaskList success itemsPerPage 5 and page 2
	Given there is GetAllTaskListCommand with itemsPerPage 5 and page 2
		And user has permission for 7 TaskLists
	When ITaskListService.GetAllAsync method executed
	Then this method should return collection with 2 elements
	
Scenario: GetAll TaskList user doesn't have permission
	Given there is GetAllTaskListCommand with itemsPerPage 4 and page 1
		And user has permission for 0 TaskLists
	When ITaskListService.GetAllAsync method executed
	Then this method should return collection with 0 elements
	
Scenario: AddConnection success
	Given there is AddConnectionCommand
		And user has permission
		And repository GetByIdAsync mock
		And repository AddConnectionAsync mock
	When ITaskListService.AddConnectionAsync method executed
	Then connection should be successfully added
	
Scenario: AddConnection user doesn't have permission
	Given there is AddConnectionCommand
		And user doesn't have permission
		And repository GetByIdAsync mock
	When ITaskListService.AddConnectionAsync method executed
	Then connection should not be added
	
Scenario: DeleteConnection success
	Given there is DeleteConnectionCommand
		And user has permission
		And repository GetByIdAsync mock
		And repository DeleteConnectionAsync mock
	When ITaskListService.DeleteConnectionAsync method executed
	Then connection should be successfully deleted
	
Scenario: DeleteConnection user doesn't have permission
	Given there is DeleteConnectionCommand
		And user doesn't have permission
		And repository GetByIdAsync mock
	When ITaskListService.DeleteConnectionAsync method executed
	Then connection should not be deleted
