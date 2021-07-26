using System;
using System.ServiceModel;

namespace Client
{
    /// <summary>
    /// интерфейс, отвечающий за логин в системе
    /// </summary>
    [ServiceContract]
    public interface ILoginInterface
    {
        /// <summary>
        /// метод пингующий сервер
        /// </summary>
        /// <returns>возвращает ответ в случае успешного пинга</returns>
        [OperationContract]
        string ping();
        //[OperationContract]
        //bool MessageSend(string Login, string random, string message, int count);
        //[OperationContract]
        //string MessageGet(string Login, string random, string message);
        [OperationContract]
        byte[] GetInformation(byte[] randomArr, byte[] query);
        //[OperationContract]
        //byte[] SendEvent(byte[] randomArr, byte[] query, int EventOrder);
        //[OperationContract]
        //byte[] GetEventResult(byte[] randomArr, int EventID);
        [OperationContract]
        byte[] CreateAccount(byte[] LoginArr, byte[] PasswordArr, byte[] NameArr);
        [OperationContract]
        byte[] LoginPhase1(byte[] LoginArr);
        [OperationContract]
        byte[] LoginPhase2(byte[] LoginArr, byte[] CryptPasswordArr);
        [OperationContract]
        byte[] LoadFile(byte[] randomArr, string FileName);
        [OperationContract]
        byte[] ProcessEvent(byte[] randomArr, byte[] query, int EventOrder);
        [OperationContract]
        byte[] GetEventResult(byte[] randomArr, int EventID);
        [OperationContract]
        byte[] GetBattleStartInfo(byte[] battleID);
        [OperationContract]
        byte[] GetBattleBasicInfo(byte[] battleID);
        [OperationContract]
        byte[] GetBattleMoveList(byte[] battleID);
        [OperationContract]
        byte[] SetBattleMoveList(byte[] randomArr, byte[] battleID, byte[] side1moves, byte[] side2moves);
        [OperationContract]
        byte[] GetBattleTurnEndTime(byte[] battleID);
        [OperationContract]
        ///<summary> метод устанавливает бой в автоматический режим</summary>
        byte[] SetBattleToAutoMode(byte[] battleID, byte[] randomArr);
        [OperationContract]
        /// <summary> получение от сервера информации о награде </summary>
        byte[] GetBattleRewardInfo(byte[] battleID);
        [OperationContract]
        /// <summary> Метод запускает тестовый бой и возвращает id боя</summary>
        byte[] StartTestBattle(byte[] array);
        [OperationContract]
        /// <summary> метод задаёт ходы для тестового боя </summary>
        byte[] SetTestBattleMoveList(byte[] battleID, byte[] sidemoves);
    }
}