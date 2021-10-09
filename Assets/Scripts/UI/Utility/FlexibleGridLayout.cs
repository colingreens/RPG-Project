using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Utility
{
    public class FlexibleGridLayout : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        [SerializeField] private FitType fitType;
        [Min(1)]
        [SerializeField] private int rows;
        [Min(1)]
        [SerializeField] private int columns;
        [Space]
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private bool fitX;
        [SerializeField] private bool fitY;

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
            {
                fitX = true;
                fitY = true;
                var sqrRt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrRt);
                columns = Mathf.CeilToInt(sqrRt);
            }
            

            if(fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                rows = Mathf.CeilToInt(transform.childCount) / columns;
            }

            if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                rows = Mathf.CeilToInt(transform.childCount) / rows;
            }

            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;

            var cellWidth = (parentWidth / (float)columns); //- ((spacing.x / (float)columns) * (columns - 1)) - (padding.left / (float)columns) - (padding.right / (float)columns);
            var cellHeight = (parentHeight / (float)rows); //- ((spacing.y / (float)rows) * 2) - (padding.top / (float)rows) - (padding.bottom / (float)rows);
            print("Cell Height  " + cellHeight);

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;
            print("Cell Sixe.Y  " + cellSize.y);

            var columnCount = 0;
            var rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;
                print("Row Count:  " + rowCount);

                var item = rectChildren[i];

                var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 0, yPos, cellSize.y);
            }
        }

        public override void SetLayoutHorizontal()
        {
           
        }

        public override void SetLayoutVertical()
        {
           
        }
    }
}
