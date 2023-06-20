using System.Diagnostics;
using static Game.World.Direction;
using static Game.World.Point2D;

namespace Game.World
{
	class MapEntity
	{
		public const int MAX_MOVEMENT_SPEED = POINTS_PER_TILE;
		public Entity Entity
		{ get; set; }
		public Point2D Pos
		{ get; set; }
		public Direction Dir
		{ get; set; }
		public int Speed
		{ get => MAX_MOVEMENT_SPEED / 4; } // TODO Replace with entity "SPD" stat or something
		public bool Passable
		{ get => Entity.Passable; }

		public MapEntity(Entity entity, Point2D pos)
		{
			Entity = entity;
			Pos = pos;
			Dir = TranslateDirection(Directions.None);
		}

		public MapEntity(Entity entity, Point2D pos, Direction dir)
		{
			Entity = entity;
			Pos = pos;
			Dir = dir;
		}

		public void Move()
		{
			Move(Dir);
		}

		public void Move(Direction dir)
		{
			dir.Normalize();
			Debug.Assert(dir.Mag <= MAX_MAG);
			Dir = dir;
			Pos = ProjectedNewLocation(dir);
		}

		public Point2D ProjectedNewLocation(Direction dir)
		{
			return Pos + EffectiveMovement(dir);
		}

		private Direction EffectiveMovement(Direction dir)
		{
			return (dir * Speed) / MAX_MOVEMENT_SPEED;
		}

		public override string ToString()
		{
			return Entity.ToString();
		}
	}

}
