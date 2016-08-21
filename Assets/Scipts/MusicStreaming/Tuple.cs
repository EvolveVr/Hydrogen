namespace Hydrogen
{
	public class Tuple 
	{
		private int _id;
		private string _trackPath;

		#region Constructor

		public Tuple(int id, string trackPath)
		{
			_id = id;
			_trackPath = trackPath;
		}

		#endregion


		#region Properties
		public int Id {
			get {
				return _id;
			}
		}

		public string TrackPath {
			get {
				return _trackPath;
			}
		}
		#endregion

	}
}
