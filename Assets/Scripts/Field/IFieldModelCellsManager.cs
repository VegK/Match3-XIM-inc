using Match3.Element;

namespace Match3.Field
{
	public interface IFieldModelCellsManager
	{
		int Width { get; }
		int Height { get; }
		void SwapRight();
		void SwapLeft();
		void SwapUp();
		void SwapDown();
		void SwapFinish();
	}
}
