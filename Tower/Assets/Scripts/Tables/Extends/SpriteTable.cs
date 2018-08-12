namespace DreamEngine.Table
{
    public partial class SpritesRow
    {
        public SpritesRow CreateTarget
        {
            get
            {
                if (m_CreateTargetID > 0)
                    return SpritesTable.Instance[m_CreateTargetID];

                return null;
            }
        }
    }
}

