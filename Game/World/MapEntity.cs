using static Game.World.Direction;
using static Game.World.Point2D;
using Game.Combat;

namespace Game.World
{
	class MapEntity
	{
		private Direction _direction;
		private MapEntity? _target;
		private Point2D _targetPos;

		public Entity Entity
		{ get; set; }
		public Point2D Pos
		{ get; set; }
		public Direction Dir
		{
			get => IsTargeting? new Direction(Pos, TargetPos) : _direction;
			set => _direction = value;
		}
		public int MovementSpeed
		{ get => EncounterType == EncounterManager.EncounterType.Combat ? ((Unit)Entity).Stats.MovementSpeed : 0; }
		public int DetectionRange
		{ get => EncounterType == EncounterManager.EncounterType.Combat ? ((Unit)Entity).Stats.DetectionRange : 0; }
		public Point2D TargetPos
		{ get => IsTargeting ? _targetPos : Pos; set => _targetPos = value; }
		public MapEntity? Target
		{
			get => _target;
			set
			{
				_target = value;
				TargetPos = IsTargeting? _target!.Pos : Pos;
			}
		}
		public bool IsTargeting
		{ get => Target != null; }
		public bool Passable
		{ get => Entity.Passable; }
		public bool Moveable
		{ get => Entity.Moveable && MovementSpeed > 1; }
		public EncounterManager.EncounterType EncounterType
		{ get => Entity.EncounterType; }

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

		public MapEntity(Entity entity)
		{
			Entity = entity;
			Pos = new Point2D();
			Dir = TranslateDirection(Directions.None);
		}

		public void Move()
		{
			Move(Dir);
		}

		public void Move(Direction dir)
		{
			Dir = dir;
			Pos = ProjectedNewLocation(dir);
		}

		public Point2D ProjectedNewLocation(Direction dir)
		{
			return ProjectedNewLocation(Pos, dir);
		}

		public Point2D ProjectedNewLocation(Point2D pos, Direction dir)
		{
			return pos + EffectiveMovement(dir);
		}

		public bool OtherInDetectionRange(MapEntity other)
		{
			return WithinDistance(Pos, other.Pos, DetectionRange);
		}

		private Direction EffectiveMovement(Direction dir)
		{
			return ClampMagnitude(dir, MovementSpeed);
		}

		public override string ToString()
		{
			return Entity.ToString();
		}
	}
}
