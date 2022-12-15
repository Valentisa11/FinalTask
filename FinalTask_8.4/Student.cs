namespace FinalTask
{
    /// <summary>
    /// сущность Студент
    /// </summary>
    [Serializable]
    class Student
    {
        public string? Name { get; set; }
        public string? Group { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}