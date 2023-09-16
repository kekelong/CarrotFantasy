using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrot
{
    public class FactoryManager
    {
        public Dictionary<FactoryType, IFacotry> factoryDict = new Dictionary<FactoryType, IFacotry>();
        public AudioClipFactory audioClipFactory;
        public SpriteFactory spriteFactory;
        public RuntimeAnimatorControllerFactory runtimeAnimatorControllerFactory;

        public FactoryManager()
        {
            factoryDict.Add(FactoryType.UIPanelFactory, new UIPanelFactory());
            factoryDict.Add(FactoryType.UIFactory, new UIFactory());
            factoryDict.Add(FactoryType.GameFactory, new GameFactory());
            audioClipFactory = new AudioClipFactory();
            spriteFactory = new SpriteFactory();
            runtimeAnimatorControllerFactory = new RuntimeAnimatorControllerFactory();
        }
    }
}
