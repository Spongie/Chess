using System.Security.Cryptography.X509Certificates;

namespace Chess.AI
{
    public interface ISavableConfigation
    {
        string Serialize();
        void DeSerialize(string data);
    }
}
