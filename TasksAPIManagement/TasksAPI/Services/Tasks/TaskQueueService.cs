using TasksAPI.Models;



    public class TaskQueueService
    {
        private readonly Queue<Tasks<int>> _queue = new();
        private bool _isInProcess = false;

        public void EnqueueTask(Tasks<int> task)
        {
            _queue.Enqueue(task);

            if (!_isInProcess)
            {
                _isInProcess = true;
                _ = ManageQueueAsync();
            }

        }

        public async Task ManageQueueAsync()
        {

            Tasks<int> taskToBeProcessed = null;

            while (_queue.Count > 0)
            {
                taskToBeProcessed = _queue.Dequeue();

                Console.WriteLine($"Tarea siendo procesada: {taskToBeProcessed.Description}");
                await Task.Delay(2000);
                Console.WriteLine($"Tarea Completada {taskToBeProcessed.Description}");
            }

            _isInProcess = false;

        }

    }

