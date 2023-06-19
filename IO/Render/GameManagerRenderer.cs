using Game;
using IO.UI;
using System;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private readonly (char, byte, byte) BORDER_INFO_V = ('║', 15, 0);
		private readonly (char, byte, byte) BORDER_INFO_H = ('═', 15, 0);
		private int[] _borderLinesJ;
		private int[] _borderLinesI;

		public GameManager GameManager
		{ get; private set; }
		public LevelRenderer LevelManagerRenderer
		{ get; private set; }
		public GameUIManagerRenderer UIManagerRenderer
		{ get; private set; }
		public ContainerMenuManagerRenderer ContainerMenuManagerRenderer
		{ get; private set; }
		public override int SizeJ
		{ get => _borderLinesJ.Length + LevelManagerRenderer.SizeJ + GameManager.DataLog.MAX_SIZE; }
		public override int SizeI
		{ get => _borderLinesI.Length + LevelManagerRenderer.SizeI; }

		public GameManagerRenderer(GameManager gameManager)
		{
			GameManager = gameManager;
			LevelManagerRenderer = new LevelRenderer(GameManager.Level);
			UIManagerRenderer = new GameUIManagerRenderer(GameManager.UIManager, this);
			_borderLinesJ = new int[] {0, LevelManagerRenderer.SizeJ + 1, LevelManagerRenderer.SizeJ + GameManager.UIManager.DataLog.MAX_SIZE + 2 };
			_borderLinesI = new int[] {0, LevelManagerRenderer.SizeI + 1};
		}

		public override void Render(FrameBuffer buffer)
		{
			RenderBorders(buffer); // TODO Probably cache this

			switch (GameManager.State)
			{
				case GameManager.GameState.Encounter:
					RenderEncounter(buffer);
					break;
				case GameManager.GameState.World:
					RenderWorld(buffer);
					break;
			}

			UIManagerRenderer.Render(buffer);
		}

		private void RenderEncounter(FrameBuffer buffer) // TODO I probably need an EncounterRenderer
		{
			if (GameManager.EncounterManager._encounterType == Game.World.EncounterManager.EncounterType.Container)
			{
				ContainerMenuManagerRenderer = new ContainerMenuManagerRenderer(GameManager.EncounterManager._containerMenuManager);
				ContainerMenuManagerRenderer.Render(buffer);
			}
		}

		private void RenderWorld(FrameBuffer buffer)
		{
			var levelBuffer = new FrameBuffer(buffer, 1, 1);
			RenderLevelAndEntities(levelBuffer);
		}

		private void RenderLevelAndEntities(FrameBuffer buffer)
		{
			LevelManagerRenderer.Render(buffer);
			RenderPlayer(buffer);
		}

		private void RenderPlayer(FrameBuffer buffer)
		{
			LevelManagerRenderer.RenderEntity(buffer, GameManager.PlayerEntity, '@', 15);
		}

		private void RenderBorders(FrameBuffer buffer)
		{
			// Render vertical borders
			foreach (var border in _borderLinesI)
			{
				for (int j = 0; j < SizeJ; j++)
				{
					buffer[j, border] = BORDER_INFO_V;
				}
			}

			// Render horizontal borders
			foreach (var border in _borderLinesJ)
			{
				for (int i = 0; i < SizeI; i++)
				{
					buffer[border, i] = BORDER_INFO_H;
				}
			}

			// Corners
			buffer.Char[_borderLinesJ[0], _borderLinesI[0]] = '╔';
			buffer.Char[_borderLinesJ[1], _borderLinesI[0]] = '╠';
			buffer.Char[_borderLinesJ[2], _borderLinesI[0]] = '╚';
			buffer.Char[_borderLinesJ[0], _borderLinesI[1]] = '╗';
			buffer.Char[_borderLinesJ[1], _borderLinesI[1]] = '╣';
			buffer.Char[_borderLinesJ[2], _borderLinesI[1]] = '╝';
		}
	}
}
