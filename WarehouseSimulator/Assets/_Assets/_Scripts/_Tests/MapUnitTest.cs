using NUnit.Framework;
using UnityEngine;
using WarehouseSimulator.Model;
using WarehouseSimulator.Model.Enums;

namespace _Assets._Scripts._Tests
{
    [TestFixture]
    public class MapUnitTest
    {
        private string[] input3x3 = {"h 3","w 3","map","...","...","..."};
        private string[] input3x4Obstacles = {"h 3","w 4","map","@...",".?..",".a.."};
        
        [Test]
        public void CreateMap_Normal()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(3, map.MapSize.y);
            Assert.AreEqual(4, map.MapSize.x);
        }
        [Test]
        public void CreateMap_CalledMultipleTimes()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            map.CreateMap(input3x3);
            Assert.AreEqual(4, map.MapSize.x);
        }
        [Test]
        public void CreateMap_InvalidTooShort()
        {
            string[] inputInvalidTooShort = {"h 3", "w 2"};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidTooShort));
        }
        [Test]
        public void CreateMap_InvalidMissingHeightValue()
        {
            string[] inputInvalidMissingHeightValue = {"h","w 3","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidMissingHeightValue));
        }
        [Test]
        public void CreateMap_InvalidMissingWidthValue()
        {
            string[] inputInvalidMissingWidthValue = {"h 3","w","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidMissingWidthValue));
        }
        [Test]
        public void CreateMap_InvalidHeightZero()
        {
            string[] inputInvalidZeroHeight = {"h 0","w 3","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidZeroHeight));
        }
        [Test]
        public void CreateMap_InvalidWidthZero()
        {
            string[] inputInvalidZeroWidth = {"h 3","w 0","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidZeroWidth));
        }
        [Test]
        public void CreateMap_InvalidHeightNegative()
        {
            string[] inputInvalidNegativeHeight = {"h -1","w 3","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidNegativeHeight));
        }
        [Test]
        public void CreateMap_InvalidWidthNegative()
        {
            string[] inputInvalidNegativeWidth = {"h 3","w -1","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidNegativeWidth));
        }
        [Test]
        public void CreateMap_InvalidHeightNotANumber()
        {
            string[] inputInvalidHeightNotANumber = {"h a","w 3","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidHeightNotANumber));
        }
        [Test]
        public void CreateMap_InvalidWidthNotANumber()
        {
            string[] inputInvalidWidthNotANumber = {"h 3","w a","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidWidthNotANumber));
        }
        [Test]
        public void CreateMap_InvalidTooSmallLineCount()
        {
            string[] inputInvalidLineCount = {"h 4","w 3","map","...",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidLineCount));
        }
        [Test]
        public void CreateMap_InvalidLineLength()
        {
            string[] inputInvalidLineLength = {"h 2","w 3","map","....",".@.","..."};
            Map map = new Map();
            Assert.Throws<InvalidFileException>(() => map.CreateMap(inputInvalidLineLength));
        }
        
        
        [Test]
        public void MapSize_Default()
        {
            Map map = new Map();
            Assert.AreEqual(Vector2Int.zero, map.MapSize);
        }
        [Test]
        public void MapSize_Normal()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            Assert.AreEqual(new Vector2Int(4,3), map.MapSize);
        }
        
        [Test]
        public void GetTileAtVec_Normal()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0,0)));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(1,0)));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(2,0)));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(1,1)));
        }
        public void GetTileAtVec_OutOfBounds()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(-1,0)));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0,-1)));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(3,0)));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0, 3)));
        }
        [Test]
        public void GetTileAtInt_Normal()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            Assert.AreEqual(TileType.Wall, map.GetTileAt(0));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(1));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(2));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(3));
            
            Assert.AreEqual(TileType.Empty, map.GetTileAt(4));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(5));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(6));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(7));
            
            Assert.AreEqual(TileType.Empty, map.GetTileAt(8));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(9));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(10));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(11));
        }
        [Test]
        public void GetTileAtInt_OutOfBounds()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            Assert.AreEqual(TileType.Wall, map.GetTileAt(-1));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(3*4));
        }
        
        
        [Test]
        public void OccupyTile_Normal()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(0,1)));
            map.OccupyTile(new Vector2Int(0,1));
            Assert.AreEqual(TileType.RoboOccupied, map.GetTileAt(new Vector2Int(0,1)));
        }
        [Test]
        public void OccupyTile_AlreadyOccupied()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(0,1)));
            map.OccupyTile(new Vector2Int(0,1));
            Assert.AreEqual(TileType.RoboOccupied, map.GetTileAt(new Vector2Int(0,1)));
            map.OccupyTile(new Vector2Int(0,1));
            Assert.AreEqual(TileType.RoboOccupied, map.GetTileAt(new Vector2Int(0,1)));
        }
        [Test]
        public void OccupyTile_Wall()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0,0)));
            map.OccupyTile(new Vector2Int(0,0));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0,0)));
        }
        [Test]
        public void OccupyTile_OutOfBounds()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(-1,0)));
            map.OccupyTile(new Vector2Int(-1,0));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(-1,0)));
        }
        
        [Test]
        public void DeoccupyTile_Normal()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(0,1)));
            map.OccupyTile(new Vector2Int(0,1));
            Assert.AreEqual(TileType.RoboOccupied, map.GetTileAt(new Vector2Int(0,1)));
            map.DeoccupyTile(new Vector2Int(0,1));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(0,1)));
        }
        [Test]
        public void DeoccupyTile_NotOccupied()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(0,1)));
            map.DeoccupyTile(new Vector2Int(0,1));
            Assert.AreEqual(TileType.Empty, map.GetTileAt(new Vector2Int(0,1)));
        }
        [Test]
        public void DeoccupyTile_Wall()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0,0)));
            map.DeoccupyTile(new Vector2Int(0,0));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(0,0)));
        }
        [Test]
        public void DeoccupyTile_OutOfBounds()
        {
            Map map = new Map();
            map.CreateMap(input3x4Obstacles);
            
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(-1,0)));
            map.DeoccupyTile(new Vector2Int(-1,0));
            Assert.AreEqual(TileType.Wall, map.GetTileAt(new Vector2Int(-1,0)));
        }
    }
}