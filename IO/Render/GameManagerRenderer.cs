using Game;
using System;
using System.Diagnostics;

namespace IO.Render
{
	class GameManagerRenderer : Renderer
	{
		private readonly (char, byte, byte) BORDER_INFO_V = ('║', 15, 0);
		private readonly (char, byte, byte) BORDER_INFO_H = ('═', 15, 0);
		private readonly (char, byte, byte) EMPTY_CHAR = (' ', 15, 0);
		private int[] _borderLinesJ;
		private int[] _borderLinesI;

		private GameManager GameManager
		{ get; set; }
		private LevelManagerRenderer LevelManagerRenderer
		{ get; set; }
		public override int SizeJ
		{ get => _borderLinesJ.Length + LevelManagerRenderer.SizeJ + GameManager.DATALOG_LENGTH; }
		public override int SizeI
		{ get => _borderLinesI.Length + LevelManagerRenderer.SizeI; }

		public GameManagerRenderer(GameManager gameManager)
		{
			GameManager = gameManager;
			LevelManagerRenderer = new LevelManagerRenderer(GameManager.LevelManager);
			_borderLinesJ = new int[] {0, LevelManagerRenderer.SizeJ + 1, LevelManagerRenderer.SizeJ + GameManager.DATALOG_LENGTH + 2 };
			_borderLinesI = new int[] {0, LevelManagerRenderer.SizeI + 1};
		}

		public override void Render(ref FrameBuffer buffer)
		{
			RenderBorders(ref buffer); // TODO Probably cache this
			var levelBuffer = new FrameBuffer(buffer, 1, 1);
			RenderLevelAndEntities(ref levelBuffer);
			var dataLogBuffer = new FrameBuffer(levelBuffer, _borderLinesJ[1], 0);
			RenderDataLog(ref dataLogBuffer);
		}

		private void RenderLevelAndEntities(ref FrameBuffer buffer)
		{
			LevelManagerRenderer.Render(ref buffer);
			RenderPlayer(ref buffer);
		}

		private void RenderPlayer(ref FrameBuffer buffer)
		{
			LevelManagerRenderer.RenderEntity(ref buffer, GameManager.PlayerEntity, '@', 15);
		}

		private void RenderDataLog(ref FrameBuffer buffer) // TODO Ugly, remove magic numbers
		{
			int j = 0;

			foreach (var line in GameManager.DataLog)
			{
				int i = 0;

				for (;  i < line.Length; i++)
					buffer[j, i] = (line[i], 15, 0);

				for (; i < LevelManagerRenderer.SizeI; i++)
					buffer[j, i] = EMPTY_CHAR;
				j++;
			}
		}

		private void RenderBorders(ref FrameBuffer buffer)
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
