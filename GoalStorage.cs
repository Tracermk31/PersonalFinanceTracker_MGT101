using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PersonalFianceTracker
{
	internal class GoalStorage
	{
		private readonly string filePath;

		private readonly List<Goal> goals;

		private readonly JsonSerializerOptions jsonOptions =
			new JsonSerializerOptions
			{
				WriteIndented = true
			};

		public GoalStorage()
		{
			string folder = Path.Combine(
				Environment.GetFolderPath(
					Environment.SpecialFolder.LocalApplicationData),
				"PersonalFianceTracker");

			Directory.CreateDirectory(folder);

			filePath = Path.Combine(folder, "goals.json");

			if (File.Exists(filePath))
			{
				string json = File.ReadAllText(filePath);

				goals = JsonSerializer.Deserialize<List<Goal>>(
					json, jsonOptions) ?? new List<Goal>();
			}
			else
			{
				goals = new List<Goal>();
			}
		}

		public IReadOnlyList<Goal> GetGoals()
		{
			return goals.AsReadOnly();
		}

		public void AddGoal(Goal goal)
		{
			goals.Add(goal);

			try
			{
				string json = JsonSerializer.Serialize(
					goals, jsonOptions);

				string tempPath = filePath + ".tmp";

				File.WriteAllText(tempPath, json);

				File.Move(tempPath, filePath, true);
			}
			catch
			{
				goals.Remove(goal);
				throw;
			}
		}
	}
}