using static Game.World.Direction;
using static Game.World.Point2D;
using Game.Combat;

namespace Game.World
{
	class LevelEntity
	{
		private Direction _direction;
		private LevelEntity? _target;
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
		public LevelEntity? Target
		{
			get => _target;
			set
			{
				_target = value;
				TargetPos = IsTargeting? _target!.Pos : Pos;
			}
		}
		public bool IsTargeting
		{ get => Target != (LevelEntity?)null; }
		public bool Passable
		{ get => Entity.Passable; }
		public bool Moveable
		{ get => Entity.Moveable && MovementSpeed > 1; }
		public virtual bool RequiresInteraction
		{ get => Entity.RequiresInteraction; }
		public EncounterManager.EncounterType EncounterType
		{ get => Entity.EncounterType; }

		public LevelEntity(Entity entity, Point2D pos)
		{
			Entity = entity;
			Pos = pos;
			Dir = TranslateDirection(Directions.None);
		}

		public LevelEntity(Entity entity, Point2D pos, Direction dir)
		{
			Entity = entity;
			Pos = pos;
			Dir = dir;
		}

		public LevelEntity(Entity entity)
		{
			Entity = entity;
			Pos = new Point2D();
			Dir = TranslateDirection(Directions.None);
		}

		public static bool operator ==(LevelEntity e, Unit u)
		{
			return e.Entity == u;
		}

		public static bool operator !=(LevelEntity e, Unit u)
		{
			return !(e == u);
		}

		public static bool operator ==(LevelEntity? e1, LevelEntity? e2)
		{
			if (e1 is not null & e2 is not null)
				return e1!.Entity == e2!.Entity;
			else if (e1 is null & e2 is null)
				return true;
			else
				return false;
		}

		public static bool operator !=(LevelEntity? e1, LevelEntity? e2)
			=> !(e1 == e2);

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

		public bool OtherInDetectionRange(LevelEntity other)
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
