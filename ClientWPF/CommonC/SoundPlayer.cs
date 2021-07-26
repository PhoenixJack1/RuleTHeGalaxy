using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
namespace Client
{
    /*
    public class DirectSoundOut : IWavePlayer
    {
        public event EventHandler<StoppedEventArgs> PlaybackStopped;

        private PlaybackState playbackState;
        private WaveFormat waveFormat;
        private int samplesTotalSize;
        private int samplesFrameSize;
        private int nextSamplesWriteIndex;
        private int desiredLatency;
        private Guid device;
        private byte[] samples;
        private IWaveProvider waveStream = null;
        private IDirectSound directSound = null;
        private IDirectSoundBuffer primarySoundBuffer = null;
        private IDirectSoundBuffer secondaryBuffer = null;
        private EventWaitHandle frameEventWaitHandle1;
        private EventWaitHandle frameEventWaitHandle2;
        private EventWaitHandle endEventWaitHandle;
        private Thread notifyThread;
        private SynchronizationContext syncContext;
        private long bytesPlayed;
        private Object m_LockObject = new Object();

        public static IEnumerable<DirectSoundDeviceInfo> Devices
        {
            get
            {
                devices = new List<DirectSoundDeviceInfo>();
                DirectSoundEnumerate(new DSEnumCallback(EnumCallback), IntPtr.Zero);
                return devices;
            }
        }

        private static List<DirectSoundDeviceInfo> devices;

        private static bool EnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpcstrModule, IntPtr lpContext)
        {
            var device = new DirectSoundDeviceInfo();
            if (lpGuid == IntPtr.Zero)
            {
                device.Guid = Guid.Empty;
            }
            else
            {
                byte[] guidBytes = new byte[16];
                Marshal.Copy(lpGuid, guidBytes, 0, 16);
                device.Guid = new Guid(guidBytes);
            }
            device.Description = Marshal.PtrToStringAnsi(lpcstrDescription);
            if (lpcstrModule != null)
            {
                device.ModuleName = Marshal.PtrToStringAnsi(lpcstrModule);
            }
            devices.Add(device);
            return true;
        }

        public DirectSoundOut()
            : this(DSDEVID_DefaultPlayback)
        {
        }
        public DirectSoundOut(Guid device)
            : this(device, 40)
        {
        }

        public DirectSoundOut(int latency)
            : this(DSDEVID_DefaultPlayback, latency)
        {
        }

        public DirectSoundOut(Guid device, int latency)
        {
            if (device == Guid.Empty)
            {
                device = DSDEVID_DefaultPlayback;
            }
            this.device = device;
            this.desiredLatency = latency;
            this.syncContext = SynchronizationContext.Current;
        }
        ~DirectSoundOut()
        {
            Dispose();
        }
        public void Play()
        {
            if (playbackState == PlaybackState.Stopped)
            {
                notifyThread = new Thread(new ThreadStart(PlaybackThreadFunc));
                notifyThread.Priority = ThreadPriority.Normal;
                notifyThread.IsBackground = true;
                notifyThread.Start();
            }

            lock (m_LockObject)
            {
                playbackState = PlaybackState.Playing;
            }
        }
        public void Stop()
        {
            if (Monitor.TryEnter(m_LockObject, 50))
            {
                playbackState = PlaybackState.Stopped;
                Monitor.Exit(m_LockObject);
            }
            else
            {
                if (notifyThread != null)
                {
                    notifyThread.Abort();
                    notifyThread = null;
                }
            }
        }
        public void Pause()
        {
            lock (m_LockObject)
            {
                playbackState = PlaybackState.Paused;
            }
        }
        public long GetPosition()
        {
            if (playbackState != PlaybackState.Stopped)
            {
                var sbuf = secondaryBuffer;
                if (sbuf != null)
                {
                    uint currentPlayCursor, currentWriteCursor;
                    sbuf.GetCurrentPosition(out currentPlayCursor, out currentWriteCursor);
                    return currentPlayCursor + bytesPlayed;
                }
            }
            return 0;
        }
        public TimeSpan PlaybackPosition
        {
            get
            {
                var pos = GetPosition();
                pos /= waveFormat.Channels * waveFormat.BitsPerSample / 8;
                return TimeSpan.FromMilliseconds(pos * 1000.0 / waveFormat.SampleRate);
            }
        }
        public void Init(IWaveProvider waveProvider)
        {
            this.waveStream = waveProvider;
            this.waveFormat = waveProvider.WaveFormat;
        }

        private void InitializeDirectSound()
        {
            lock (this.m_LockObject)
            {
                directSound = null;
                DirectSoundCreate(ref device, out directSound, IntPtr.Zero);

                if (directSound != null)
                {
                    directSound.SetCooperativeLevel(GetDesktopWindow(), DirectSoundCooperativeLevel.DSSCL_PRIORITY);
                    BufferDescription bufferDesc = new BufferDescription();
                    bufferDesc.dwSize = Marshal.SizeOf(bufferDesc);
                    bufferDesc.dwBufferBytes = 0;
                    bufferDesc.dwFlags = DirectSoundBufferCaps.DSBCAPS_PRIMARYBUFFER;
                    bufferDesc.dwReserved = 0;
                    bufferDesc.lpwfxFormat = IntPtr.Zero;
                    bufferDesc.guidAlgo = Guid.Empty;

                    object soundBufferObj;
                    directSound.CreateSoundBuffer(bufferDesc, out soundBufferObj, IntPtr.Zero);
                    primarySoundBuffer = (IDirectSoundBuffer)soundBufferObj;
                    primarySoundBuffer.Play(0, 0, DirectSoundPlayFlags.DSBPLAY_LOOPING);

                    samplesFrameSize = MsToBytes(desiredLatency);

                    BufferDescription bufferDesc2 = new BufferDescription();
                    bufferDesc2.dwSize = Marshal.SizeOf(bufferDesc2);
                    bufferDesc2.dwBufferBytes = (uint)(samplesFrameSize * 2);
                    bufferDesc2.dwFlags = DirectSoundBufferCaps.DSBCAPS_GETCURRENTPOSITION2
                        | DirectSoundBufferCaps.DSBCAPS_CTRLPOSITIONNOTIFY
                        | DirectSoundBufferCaps.DSBCAPS_GLOBALFOCUS
                        | DirectSoundBufferCaps.DSBCAPS_CTRLVOLUME
                        | DirectSoundBufferCaps.DSBCAPS_STICKYFOCUS
                        | DirectSoundBufferCaps.DSBCAPS_GETCURRENTPOSITION2;
                    bufferDesc2.dwReserved = 0;
                    GCHandle handleOnWaveFormat = GCHandle.Alloc(waveFormat, GCHandleType.Pinned); // Ptr to waveFormat
                    bufferDesc2.lpwfxFormat = handleOnWaveFormat.AddrOfPinnedObject(); // set Ptr to waveFormat
                    bufferDesc2.guidAlgo = Guid.Empty;

                    directSound.CreateSoundBuffer(bufferDesc2, out soundBufferObj, IntPtr.Zero);
                    secondaryBuffer = (IDirectSoundBuffer)soundBufferObj;
                    handleOnWaveFormat.Free();
                    BufferCaps dsbCaps = new BufferCaps();
                    dsbCaps.dwSize = Marshal.SizeOf(dsbCaps);
                    secondaryBuffer.GetCaps(dsbCaps);

                    nextSamplesWriteIndex = 0;
                    samplesTotalSize = dsbCaps.dwBufferBytes;
                    samples = new byte[samplesTotalSize];
                    System.Diagnostics.Debug.Assert(samplesTotalSize == (2 * samplesFrameSize), "Invalid SamplesTotalSize vs SamplesFrameSize");
                    IDirectSoundNotify notify = (IDirectSoundNotify)soundBufferObj;

                    frameEventWaitHandle1 = new EventWaitHandle(false, EventResetMode.AutoReset);
                    frameEventWaitHandle2 = new EventWaitHandle(false, EventResetMode.AutoReset);
                    endEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

                    DirectSoundBufferPositionNotify[] notifies = new DirectSoundBufferPositionNotify[3];
                    notifies[0] = new DirectSoundBufferPositionNotify();
                    notifies[0].dwOffset = 0;
                    notifies[0].hEventNotify = frameEventWaitHandle1.SafeWaitHandle.DangerousGetHandle();

                    notifies[1] = new DirectSoundBufferPositionNotify();
                    notifies[1].dwOffset = (uint)samplesFrameSize;
                    notifies[1].hEventNotify = frameEventWaitHandle2.SafeWaitHandle.DangerousGetHandle();

                    notifies[2] = new DirectSoundBufferPositionNotify();
                    notifies[2].dwOffset = 0xFFFFFFFF;
                    notifies[2].hEventNotify = endEventWaitHandle.SafeWaitHandle.DangerousGetHandle();

                    notify.SetNotificationPositions(3, notifies);
                }
            }
        }

        public PlaybackState PlaybackState
        {
            get { return playbackState; }
        }

        public float Volume
        {
            get
            {
                return 1.0f;
            }
            set
            {
                if (value != 1.0f)
                {
                    throw new InvalidOperationException("Setting volume not supported on DirectSoundOut, adjust the volume on your WaveProvider instead");
                }
            }
        }

        public void Dispose()
        {
            Stop();
            GC.SuppressFinalize(this);
        }
        private bool IsBufferLost()
        {
            return (secondaryBuffer.GetStatus() & DirectSoundBufferStatus.DSBSTATUS_BUFFERLOST) != 0 ? true : false;
        }

        private int MsToBytes(int ms)
        {
            int bytes = ms * (waveFormat.AverageBytesPerSecond / 1000);
            bytes -= bytes % waveFormat.BlockAlign;
            return bytes;
        }

        private void PlaybackThreadFunc()
        {
            bool lPlaybackHalted = false;
            bool firstBufferStarted = false;
            bytesPlayed = 0;

            Exception exception = null;
            try
            {
                InitializeDirectSound();
                int lResult = 1;

                if (PlaybackState == PlaybackState.Stopped)
                {
                    secondaryBuffer.SetCurrentPosition(0);
                    nextSamplesWriteIndex = 0;
                    lResult = Feed(samplesTotalSize);
                }

                if (lResult > 0)
                {
                    lock (m_LockObject)
                    {
                        playbackState = PlaybackState.Playing;
                    }

                    secondaryBuffer.Play(0, 0, DirectSoundPlayFlags.DSBPLAY_LOOPING);

                    var waitHandles = new WaitHandle[] { frameEventWaitHandle1, frameEventWaitHandle2, endEventWaitHandle };

                    bool lContinuePlayback = true;
                    while (PlaybackState != PlaybackState.Stopped && lContinuePlayback)
                    {

                        int indexHandle = WaitHandle.WaitAny(waitHandles, 3 * desiredLatency, false);
                        if (indexHandle != WaitHandle.WaitTimeout)
                        {
                            if (indexHandle == 2)
                            {
                                StopPlayback();
                                lPlaybackHalted = true;
                                lContinuePlayback = false;
                            }
                            else
                            {
                                if (indexHandle == 0)
                                {
                                    if (firstBufferStarted)
                                    {
                                        bytesPlayed += samplesFrameSize * 2;
                                    }
                                }
                                else
                                {
                                    firstBufferStarted = true;
                                }

                                indexHandle = (indexHandle == 0) ? 1 : 0;
                                nextSamplesWriteIndex = indexHandle * samplesFrameSize;

                                if (Feed(samplesFrameSize) == 0)
                                {
                                    StopPlayback();
                                    lPlaybackHalted = true;
                                    lContinuePlayback = false;
                                }
                            }
                        }
                        else
                        {
                            // Timed out!
                            StopPlayback();
                            lPlaybackHalted = true;
                            lContinuePlayback = false;
                            throw new Exception("DirectSound buffer timeout");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                exception = e;
            }
            finally
            {
                if (!lPlaybackHalted)
                {
                    try
                    {
                        StopPlayback();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                        if (exception == null) exception = e;
                    }
                }

                lock (m_LockObject)
                {
                    playbackState = PlaybackState.Stopped;
                }

                bytesPlayed = 0;
                RaisePlaybackStopped(exception);
            }
        }

        private void RaisePlaybackStopped(Exception e)
        {
            var handler = PlaybackStopped;
            if (handler != null)
            {
                if (this.syncContext == null)
                {
                    handler(this, new StoppedEventArgs(e));
                }
                else
                {
                    syncContext.Post(state => handler(this, new StoppedEventArgs(e)), null);
                }
            }
        }

        private void StopPlayback()
        {
            lock (this.m_LockObject)
            {
                if (secondaryBuffer != null)
                {
                    secondaryBuffer.Stop();
                    secondaryBuffer = null;
                }
                if (primarySoundBuffer != null)
                {
                    primarySoundBuffer.Stop();
                    primarySoundBuffer = null;
                }
            }
        }
        private int Feed(int bytesToCopy)
        {
            int bytesRead = bytesToCopy;
            if (IsBufferLost())
            {
                secondaryBuffer.Restore();
            }
            if (playbackState == PlaybackState.Paused)
            {
                Array.Clear(samples, 0, samples.Length);
            }
            else
            {
                bytesRead = waveStream.Read(samples, 0, bytesToCopy);

                if (bytesRead == 0)
                {
                    Array.Clear(samples, 0, samples.Length);
                    return 0;
                }
            }

            IntPtr wavBuffer1;
            int nbSamples1;
            IntPtr wavBuffer2;
            int nbSamples2;
            secondaryBuffer.Lock(nextSamplesWriteIndex, (uint)bytesRead,
                                 out wavBuffer1, out nbSamples1,
                                 out wavBuffer2, out nbSamples2,
                                 DirectSoundBufferLockFlag.None);

            if (wavBuffer1 != IntPtr.Zero)
            {
                Marshal.Copy(samples, 0, wavBuffer1, nbSamples1);
                if (wavBuffer2 != IntPtr.Zero)
                {
                    Marshal.Copy(samples, 0, wavBuffer1, nbSamples1);
                }
            }

            secondaryBuffer.Unlock(wavBuffer1, nbSamples1, wavBuffer2, nbSamples2);

            return bytesRead;
        }


        #region Native DirectSound COM Interface

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class BufferDescription
        {
            public int dwSize;
            [MarshalAs(UnmanagedType.U4)]
            public DirectSoundBufferCaps dwFlags;
            public uint dwBufferBytes;
            public int dwReserved;
            public IntPtr lpwfxFormat;
            public Guid guidAlgo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal class BufferCaps
        {
            public int dwSize;
            public int dwFlags;
            public int dwBufferBytes;
            public int dwUnlockTransferRate;
            public int dwPlayCpuOverhead;
        }

        internal enum DirectSoundCooperativeLevel : uint
        {
            DSSCL_NORMAL = 0x00000001,
            DSSCL_PRIORITY = 0x00000002,
            DSSCL_EXCLUSIVE = 0x00000003,
            DSSCL_WRITEPRIMARY = 0x00000004
        }

        [FlagsAttribute]
        internal enum DirectSoundPlayFlags : uint
        {
            DSBPLAY_LOOPING = 0x00000001,
            DSBPLAY_LOCHARDWARE = 0x00000002,
            DSBPLAY_LOCSOFTWARE = 0x00000004,
            DSBPLAY_TERMINATEBY_TIME = 0x00000008,
            DSBPLAY_TERMINATEBY_DISTANCE = 0x000000010,
            DSBPLAY_TERMINATEBY_PRIORITY = 0x000000020
        }

        internal enum DirectSoundBufferLockFlag : uint
        {
            None = 0,
            FromWriteCursor = 0x00000001,
            EntireBuffer = 0x00000002
        }

        [FlagsAttribute]
        internal enum DirectSoundBufferStatus : uint
        {
            DSBSTATUS_PLAYING = 0x00000001,
            DSBSTATUS_BUFFERLOST = 0x00000002,
            DSBSTATUS_LOOPING = 0x00000004,
            DSBSTATUS_LOCHARDWARE = 0x00000008,
            DSBSTATUS_LOCSOFTWARE = 0x00000010,
            DSBSTATUS_TERMINATED = 0x00000020
        }

        [FlagsAttribute]
        internal enum DirectSoundBufferCaps : uint
        {
            DSBCAPS_PRIMARYBUFFER = 0x00000001,
            DSBCAPS_STATIC = 0x00000002,
            DSBCAPS_LOCHARDWARE = 0x00000004,
            DSBCAPS_LOCSOFTWARE = 0x00000008,
            DSBCAPS_CTRL3D = 0x00000010,
            DSBCAPS_CTRLFREQUENCY = 0x00000020,
            DSBCAPS_CTRLPAN = 0x00000040,
            DSBCAPS_CTRLVOLUME = 0x00000080,
            DSBCAPS_CTRLPOSITIONNOTIFY = 0x00000100,
            DSBCAPS_CTRLFX = 0x00000200,
            DSBCAPS_STICKYFOCUS = 0x00004000,
            DSBCAPS_GLOBALFOCUS = 0x00008000,
            DSBCAPS_GETCURRENTPOSITION2 = 0x00010000,
            DSBCAPS_MUTE3DATMAXDISTANCE = 0x00020000,
            DSBCAPS_LOCDEFER = 0x00040000
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DirectSoundBufferPositionNotify
        {
            public UInt32 dwOffset;
            public IntPtr hEventNotify;
        }

        [ComImport,
         Guid("279AFA83-4981-11CE-A521-0020AF0BE560"),
         InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
         SuppressUnmanagedCodeSecurity]
        internal interface IDirectSound
        {
            void CreateSoundBuffer([In] BufferDescription desc, [Out, MarshalAs(UnmanagedType.Interface)] out object dsDSoundBuffer, IntPtr pUnkOuter);
            void GetCaps(IntPtr caps);
            void DuplicateSoundBuffer([In, MarshalAs(UnmanagedType.Interface)] IDirectSoundBuffer bufferOriginal, [In, MarshalAs(UnmanagedType.Interface)] IDirectSoundBuffer bufferDuplicate);
            void SetCooperativeLevel(IntPtr HWND, [In, MarshalAs(UnmanagedType.U4)] DirectSoundCooperativeLevel dwLevel);
            void Compact();
            void GetSpeakerConfig(IntPtr pdwSpeakerConfig);
            void SetSpeakerConfig(uint pdwSpeakerConfig);
            void Initialize([In, MarshalAs(UnmanagedType.LPStruct)] Guid guid);
        }

        [ComImport,
         Guid("279AFA85-4981-11CE-A521-0020AF0BE560"),
         InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
         SuppressUnmanagedCodeSecurity]
        internal interface IDirectSoundBuffer
        {
            void GetCaps([MarshalAs(UnmanagedType.LPStruct)] BufferCaps pBufferCaps);
            void GetCurrentPosition([Out] out uint currentPlayCursor, [Out] out uint currentWriteCursor);
            void GetFormat();
            [return: MarshalAs(UnmanagedType.I4)]
            int GetVolume();
            void GetPan([Out] out uint pan);
            [return: MarshalAs(UnmanagedType.I4)]
            int GetFrequency();
            [return: MarshalAs(UnmanagedType.U4)]
            DirectSoundBufferStatus GetStatus();
            void Initialize([In, MarshalAs(UnmanagedType.Interface)] IDirectSound directSound, [In] BufferDescription desc);
            void Lock(int dwOffset, uint dwBytes, [Out] out IntPtr audioPtr1, [Out] out int audioBytes1, [Out] out IntPtr audioPtr2, [Out] out int audioBytes2, [MarshalAs(UnmanagedType.U4)] DirectSoundBufferLockFlag dwFlags);
            void Play(uint dwReserved1, uint dwPriority, [In, MarshalAs(UnmanagedType.U4)] DirectSoundPlayFlags dwFlags);
            void SetCurrentPosition(uint dwNewPosition);
            void SetFormat([In] WaveFormat pcfxFormat);
            void SetVolume(int volume);
            void SetPan(uint pan);
            void SetFrequency(uint frequency);
            void Stop();
            void Unlock(IntPtr pvAudioPtr1, int dwAudioBytes1, IntPtr pvAudioPtr2, int dwAudioBytes2);
            void Restore();
        }
        [ComImport,
         Guid("b0210783-89cd-11d0-af08-00a0c925cd16"),
         InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
         SuppressUnmanagedCodeSecurity]
        internal interface IDirectSoundNotify
        {
            void SetNotificationPositions(UInt32 dwPositionNotifies, [In, MarshalAs(UnmanagedType.LPArray)] DirectSoundBufferPositionNotify[] pcPositionNotifies);
        }

        [DllImport("dsound.dll", EntryPoint = "DirectSoundCreate", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern void DirectSoundCreate(ref Guid GUID, [Out, MarshalAs(UnmanagedType.Interface)] out IDirectSound directSound, IntPtr pUnkOuter);
        public static readonly Guid DSDEVID_DefaultPlayback = new Guid("DEF00000-9C6D-47ED-AAF1-4DDA8F2B5C03");
        public static readonly Guid DSDEVID_DefaultCapture = new Guid("DEF00001-9C6D-47ED-AAF1-4DDA8F2B5C03");
        public static readonly Guid DSDEVID_DefaultVoicePlayback = new Guid("DEF00002-9C6D-47ED-AAF1-4DDA8F2B5C03");
        public static readonly Guid DSDEVID_DefaultVoiceCapture = new Guid("DEF00003-9C6D-47ED-AAF1-4DDA8F2B5C03");
        delegate bool DSEnumCallback(IntPtr lpGuid, IntPtr lpcstrDescription, IntPtr lpcstrModule, IntPtr lpContext);
        [DllImport("dsound.dll", EntryPoint = "DirectSoundEnumerateA", SetLastError = true, CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        static extern void DirectSoundEnumerate(DSEnumCallback lpDSEnumCallback, IntPtr lpContext);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();
        #endregion
    }
    public class DirectSoundDeviceInfo
    {
        public Guid Guid { get; set; }
        public string Description { get; set; }
        public string ModuleName { get; set; }
    }
    class AudioMediaSubtypes
    {
        public static readonly Guid MEDIASUBTYPE_PCM = new Guid("00000001-0000-0010-8000-00AA00389B71"); // PCM audio. 
        public static readonly Guid MEDIASUBTYPE_PCMAudioObsolete = new Guid("e436eb8a-524f-11ce-9f53-0020af0ba770"); // Obsolete. Do not use. 
        public static readonly Guid MEDIASUBTYPE_MPEG1Packet = new Guid("e436eb80-524f-11ce-9f53-0020af0ba770"); // MPEG1 Audio packet. 
        public static readonly Guid MEDIASUBTYPE_MPEG1Payload = new Guid("e436eb81-524f-11ce-9f53-0020af0ba770"); // MPEG1 Audio Payload. 
        public static readonly Guid MEDIASUBTYPE_MPEG2_AUDIO = new Guid("e06d802b-db46-11cf-b4d1-00805f6cbbea"); // MPEG-2 audio data  
        public static readonly Guid MEDIASUBTYPE_DVD_LPCM_AUDIO = new Guid("e06d8032-db46-11cf-b4d1-00805f6cbbea"); // DVD audio data  
        public static readonly Guid MEDIASUBTYPE_DRM_Audio = new Guid("00000009-0000-0010-8000-00aa00389b71"); // Corresponds to WAVE_FORMAT_DRM. 
        public static readonly Guid MEDIASUBTYPE_IEEE_FLOAT = new Guid("00000003-0000-0010-8000-00aa00389b71"); // Corresponds to WAVE_FORMAT_IEEE_FLOAT 
        public static readonly Guid MEDIASUBTYPE_DOLBY_AC3 = new Guid("e06d802c-db46-11cf-b4d1-00805f6cbbea"); // Dolby data  
        public static readonly Guid MEDIASUBTYPE_DOLBY_AC3_SPDIF = new Guid("00000092-0000-0010-8000-00aa00389b71"); // Dolby AC3 over SPDIF.  
        public static readonly Guid MEDIASUBTYPE_RAW_SPORT = new Guid("00000240-0000-0010-8000-00aa00389b71"); // Equivalent to MEDIASUBTYPE_DOLBY_AC3_SPDIF. 
        public static readonly Guid MEDIASUBTYPE_SPDIF_TAG_241h = new Guid("00000241-0000-0010-8000-00aa00389b71"); // Equivalent to MEDIASUBTYPE_DOLBY_AC3_SPDIF. 
        //http://msdn.microsoft.com/en-us/library/dd757532%28VS.85%29.aspx
        public static readonly Guid WMMEDIASUBTYPE_MP3 = new Guid("00000055-0000-0010-8000-00AA00389B71");
        // others?
        public static readonly Guid MEDIASUBTYPE_WAVE = new Guid("e436eb8b-524f-11ce-9f53-0020af0ba770");
        public static readonly Guid MEDIASUBTYPE_AU = new Guid("e436eb8c-524f-11ce-9f53-0020af0ba770");
        public static readonly Guid MEDIASUBTYPE_AIFF = new Guid("e436eb8d-524f-11ce-9f53-0020af0ba770");

        public static readonly Guid[] AudioSubTypes = new Guid[]
        {
            MEDIASUBTYPE_PCM,
            MEDIASUBTYPE_PCMAudioObsolete,
            MEDIASUBTYPE_MPEG1Packet,
            MEDIASUBTYPE_MPEG1Payload,
            MEDIASUBTYPE_MPEG2_AUDIO,
            MEDIASUBTYPE_DVD_LPCM_AUDIO,
            MEDIASUBTYPE_DRM_Audio,
            MEDIASUBTYPE_IEEE_FLOAT,
            MEDIASUBTYPE_DOLBY_AC3,
            MEDIASUBTYPE_DOLBY_AC3_SPDIF,
            MEDIASUBTYPE_RAW_SPORT,
            MEDIASUBTYPE_SPDIF_TAG_241h,
            WMMEDIASUBTYPE_MP3,
        };

        public static readonly string[] AudioSubTypeNames = new string[]
        {
            "PCM",
            "PCM Obsolete",
            "MPEG1Packet",
            "MPEG1Payload",
            "MPEG2_AUDIO",
            "DVD_LPCM_AUDIO",
            "DRM_Audio",
            "IEEE_FLOAT",
            "DOLBY_AC3",
            "DOLBY_AC3_SPDIF",
            "RAW_SPORT",
            "SPDIF_TAG_241h",
            "MP3"
        };
        public static string GetAudioSubtypeName(Guid subType)
        {
            for (int index = 0; index < AudioSubTypes.Length; index++)
            {
                if (subType == AudioSubTypes[index])
                {
                    return AudioSubTypeNames[index];
                }
            }
            return subType.ToString();
        }
    }
    public interface IWavePlayer : IDisposable
    {
        void Play();
        void Stop();
        void Pause();
        void Init(IWaveProvider waveProvider);
        PlaybackState PlaybackState { get; }
        [Obsolete("Not intending to keep supporting this going forward: set the volume on your input WaveProvider instead")]
        float Volume { get; set; }
        event EventHandler<StoppedEventArgs> PlaybackStopped;
    }
    public interface IWavePosition
    {
        long GetPosition();
        WaveFormat OutputWaveFormat { get; }
    }
    public enum PlaybackState
    {
        Stopped,
        Playing,
        Paused
    }
    public class StoppedEventArgs : EventArgs
    {
        private readonly Exception exception;
        public StoppedEventArgs(Exception exception = null)
        {
            this.exception = exception;
        }
        public Exception Exception { get { return exception; } }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class Gsm610WaveFormat : WaveFormat
    {
        private short samplesPerBlock;
        public Gsm610WaveFormat()
        {
            this.waveFormatTag = WaveFormatEncoding.Gsm610;
            this.channels = 1;
            this.averageBytesPerSecond = 1625;
            this.bitsPerSample = 0; // must be zero
            this.blockAlign = 65;
            this.sampleRate = 8000;

            this.extraSize = 2;
            this.samplesPerBlock = 320;
        }
        public short SamplesPerBlock { get { return this.samplesPerBlock; } }
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(samplesPerBlock);
        }
    }
    public interface IWaveProvider
    {
        WaveFormat WaveFormat { get; }
        int Read(byte[] buffer, int offset, int count);
    }
    public class RiffChunk
    {
        public RiffChunk(int identifier, int length, long streamPosition)
        {
            Identifier = identifier;
            Length = length;
            StreamPosition = streamPosition;
        }
        public int Identifier { get; private set; }
        public string IdentifierAsString
        {
            get
            {
                return Encoding.UTF8.GetString(BitConverter.GetBytes(Identifier));
            }
        }
        public int Length { get; private set; }
        public long StreamPosition { get; private set; }
    }
    public static class BufferHelpers
    {
        public static byte[] Ensure(byte[] buffer, int bytesRequired)
        {
            if (buffer == null || buffer.Length < bytesRequired)
            {
                buffer = new byte[bytesRequired];
            }
            return buffer;
        }
        public static float[] Ensure(float[] buffer, int samplesRequired)
        {
            if (buffer == null || buffer.Length < samplesRequired)
            {
                buffer = new float[samplesRequired];
            }
            return buffer;
        }
    }
    public interface IWaveBuffer
    {
        byte[] ByteBuffer { get; }
        float[] FloatBuffer { get; }
        short[] ShortBuffer { get; }
        int[] IntBuffer { get; }
        int MaxSize { get; }
        int ByteBufferCount { get; }
        int FloatBufferCount { get; }
        int ShortBufferCount { get; }
        int IntBufferCount { get; }
    }
    [StructLayout(LayoutKind.Explicit, Pack = 2)]
    public class WaveBuffer : IWaveBuffer
    {
        [FieldOffset(0)]
        public int numberOfBytes;
        [FieldOffset(8)]
        private byte[] byteBuffer;
        [FieldOffset(8)]
        private float[] floatBuffer;
        [FieldOffset(8)]
        private short[] shortBuffer;
        [FieldOffset(8)]
        private int[] intBuffer;
        public WaveBuffer(int sizeToAllocateInBytes)
        {
            int aligned4Bytes = sizeToAllocateInBytes % 4;
            sizeToAllocateInBytes = (aligned4Bytes == 0) ? sizeToAllocateInBytes : sizeToAllocateInBytes + 4 - aligned4Bytes;
            // Allocating the byteBuffer is co-allocating the floatBuffer and the intBuffer
            byteBuffer = new byte[sizeToAllocateInBytes];
            numberOfBytes = 0;
        }
        public WaveBuffer(byte[] bufferToBoundTo)
        {
            BindTo(bufferToBoundTo);
        }
        public void BindTo(byte[] bufferToBoundTo)
        {
            byteBuffer = bufferToBoundTo;
            numberOfBytes = 0;
        }
        public static implicit operator byte[] (WaveBuffer waveBuffer)
        {
            return waveBuffer.byteBuffer;
        }
        public static implicit operator float[] (WaveBuffer waveBuffer)
        {
            return waveBuffer.floatBuffer;
        }
        public static implicit operator int[] (WaveBuffer waveBuffer)
        {
            return waveBuffer.intBuffer;
        }
        public static implicit operator short[] (WaveBuffer waveBuffer)
        {
            return waveBuffer.shortBuffer;
        }

        public byte[] ByteBuffer
        {
            get { return byteBuffer; }
        }
        public float[] FloatBuffer
        {
            get { return floatBuffer; }
        }
        public short[] ShortBuffer
        {
            get { return shortBuffer; }
        }
        public int[] IntBuffer
        {
            get { return intBuffer; }
        }
        public int MaxSize
        {
            get { return byteBuffer.Length; }
        }
        public int ByteBufferCount
        {
            get { return numberOfBytes; }
            set
            {
                numberOfBytes = CheckValidityCount("ByteBufferCount", value, 1);
            }
        }
        public int FloatBufferCount
        {
            get { return numberOfBytes / 4; }
            set
            {
                numberOfBytes = CheckValidityCount("FloatBufferCount", value, 4);
            }
        }
        public int ShortBufferCount
        {
            get { return numberOfBytes / 2; }
            set
            {
                numberOfBytes = CheckValidityCount("ShortBufferCount", value, 2);
            }
        }
        public int IntBufferCount
        {
            get { return numberOfBytes / 4; }
            set
            {
                numberOfBytes = CheckValidityCount("IntBufferCount", value, 4);
            }
        }
        public void Clear()
        {
            Array.Clear(byteBuffer, 0, byteBuffer.Length);
        }

        public void Copy(Array destinationArray)
        {
            Array.Copy(byteBuffer, destinationArray, numberOfBytes);
        }
        private int CheckValidityCount(string argName, int value, int sizeOfValue)
        {
            int newNumberOfBytes = value * sizeOfValue;
            if ((newNumberOfBytes % 4) != 0)
            {
                throw new ArgumentOutOfRangeException(argName, String.Format("{0} cannot set a count ({1}) that is not 4 bytes aligned ", argName, newNumberOfBytes));
            }

            if (value < 0 || value > (byteBuffer.Length / sizeOfValue))
            {
                throw new ArgumentOutOfRangeException(argName, String.Format("{0} cannot set a count that exceed max count {1}", argName, byteBuffer.Length / sizeOfValue));
            }
            return newNumberOfBytes;
        }
    }
    class Mono8SampleChunkConverter : ISampleChunkConverter
    {
        private int offset;
        private byte[] sourceBuffer;
        private int sourceBytes;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 8 &&
                waveFormat.Channels == 1;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceBytes = source.Read(sourceBuffer, 0, sourceBytesRequired);
            offset = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (offset < sourceBytes)
            {
                sampleLeft = sourceBuffer[offset] / 256f;
                offset++;
                sampleRight = sampleLeft;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    class Stereo8SampleChunkConverter : ISampleChunkConverter
    {
        private int offset;
        private byte[] sourceBuffer;
        private int sourceBytes;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 8 &&
                waveFormat.Channels == 2;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 2;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceBytes = source.Read(sourceBuffer, 0, sourceBytesRequired);
            offset = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (offset < sourceBytes)
            {
                sampleLeft = sourceBuffer[offset++] / 256f;
                sampleRight = sourceBuffer[offset++] / 256f;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    class Mono16SampleChunkConverter : ISampleChunkConverter
    {
        private int sourceSample;
        private byte[] sourceBuffer;
        private WaveBuffer sourceWaveBuffer;
        private int sourceSamples;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 16 &&
                waveFormat.Channels == 1;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 2;
            sourceSample = 0;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceWaveBuffer = new WaveBuffer(sourceBuffer);
            sourceSamples = source.Read(sourceBuffer, 0, sourceBytesRequired) / 2;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (sourceSample < sourceSamples)
            {
                sampleLeft = sourceWaveBuffer.ShortBuffer[sourceSample++] / 32768.0f;
                sampleRight = sampleLeft;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    class Stereo16SampleChunkConverter : ISampleChunkConverter
    {
        private int sourceSample;
        private byte[] sourceBuffer;
        private WaveBuffer sourceWaveBuffer;
        private int sourceSamples;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 16 &&
                waveFormat.Channels == 2;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 4;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceWaveBuffer = new WaveBuffer(sourceBuffer);
            sourceSamples = source.Read(sourceBuffer, 0, sourceBytesRequired) / 2;
            sourceSample = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (sourceSample < sourceSamples)
            {
                sampleLeft = sourceWaveBuffer.ShortBuffer[sourceSample++] / 32768.0f;
                sampleRight = sourceWaveBuffer.ShortBuffer[sourceSample++] / 32768.0f;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    class Mono24SampleChunkConverter : ISampleChunkConverter
    {
        private int offset;
        private byte[] sourceBuffer;
        private int sourceBytes;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 24 &&
                waveFormat.Channels == 1;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 3;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceBytes = source.Read(sourceBuffer, 0, sourceBytesRequired);
            offset = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (offset < sourceBytes)
            {
                sampleLeft = (((sbyte)sourceBuffer[offset + 2] << 16) | (sourceBuffer[offset + 1] << 8) | sourceBuffer[offset]) / 8388608f;
                offset += 3;
                sampleRight = sampleLeft;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    class Stereo24SampleChunkConverter : ISampleChunkConverter
    {
        private int offset;
        private byte[] sourceBuffer;
        private int sourceBytes;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.Pcm &&
                waveFormat.BitsPerSample == 24 &&
                waveFormat.Channels == 2;
        }


        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 6;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceBytes = source.Read(sourceBuffer, 0, sourceBytesRequired);
            offset = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (offset < sourceBytes)
            {
                sampleLeft = (((sbyte)sourceBuffer[offset + 2] << 16) | (sourceBuffer[offset + 1] << 8) | sourceBuffer[offset]) / 8388608f;
                offset += 3;
                sampleRight = (((sbyte)sourceBuffer[offset + 2] << 16) | (sourceBuffer[offset + 1] << 8) | sourceBuffer[offset]) / 8388608f;
                offset += 3;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }

    }
    class MonoFloatSampleChunkConverter : ISampleChunkConverter
    {
        private int sourceSample;
        private byte[] sourceBuffer;
        private WaveBuffer sourceWaveBuffer;
        private int sourceSamples;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.IeeeFloat &&
                waveFormat.Channels == 1;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 4;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceWaveBuffer = new WaveBuffer(sourceBuffer);
            sourceSamples = source.Read(sourceBuffer, 0, sourceBytesRequired) / 4;
            sourceSample = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (sourceSample < sourceSamples)
            {
                sampleLeft = sourceWaveBuffer.FloatBuffer[sourceSample++];
                sampleRight = sampleLeft;
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    class StereoFloatSampleChunkConverter : ISampleChunkConverter
    {
        private int sourceSample;
        private byte[] sourceBuffer;
        private WaveBuffer sourceWaveBuffer;
        private int sourceSamples;

        public bool Supports(WaveFormat waveFormat)
        {
            return waveFormat.Encoding == WaveFormatEncoding.IeeeFloat &&
                waveFormat.Channels == 2;
        }

        public void LoadNextChunk(IWaveProvider source, int samplePairsRequired)
        {
            int sourceBytesRequired = samplePairsRequired * 8;
            sourceBuffer = BufferHelpers.Ensure(sourceBuffer, sourceBytesRequired);
            sourceWaveBuffer = new WaveBuffer(sourceBuffer);
            sourceSamples = source.Read(sourceBuffer, 0, sourceBytesRequired) / 4;
            sourceSample = 0;
        }

        public bool GetNextSample(out float sampleLeft, out float sampleRight)
        {
            if (sourceSample < sourceSamples)
            {
                sampleLeft = sourceWaveBuffer.FloatBuffer[sourceSample++];
                sampleRight = sourceWaveBuffer.FloatBuffer[sourceSample++];
                return true;
            }
            else
            {
                sampleLeft = 0.0f;
                sampleRight = 0.0f;
                return false;
            }
        }
    }
    public class WaveChannel32 : WaveStream, ISampleNotifier
    {
        private WaveStream sourceStream;
        private readonly WaveFormat waveFormat;
        private readonly long length;
        private readonly int destBytesPerSample;
        private readonly int sourceBytesPerSample;
        private volatile float volume;
        private volatile float pan;
        private long position;
        private readonly ISampleChunkConverter sampleProvider;
        private readonly object lockObject = new object();
        public WaveChannel32(WaveStream sourceStream, float volume, float pan)
        {
            PadWithZeroes = true;

            var providers = new ISampleChunkConverter[]
            {
                new Mono8SampleChunkConverter(),
                new Stereo8SampleChunkConverter(),
                new Mono16SampleChunkConverter(),
                new Stereo16SampleChunkConverter(),
                new Mono24SampleChunkConverter(),
                new Stereo24SampleChunkConverter(),
                new MonoFloatSampleChunkConverter(),
                new StereoFloatSampleChunkConverter(),
            };
            foreach (var provider in providers)
            {
                if (provider.Supports(sourceStream.WaveFormat))
                {
                    this.sampleProvider = provider;
                    break;
                }
            }

            if (this.sampleProvider == null)
            {
                throw new ArgumentException("Unsupported sourceStream format");
            }

            // always outputs stereo 32 bit
            waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sourceStream.WaveFormat.SampleRate, 2);
            destBytesPerSample = 8; // includes stereo factoring

            this.sourceStream = sourceStream;
            this.volume = volume;
            this.pan = pan;
            sourceBytesPerSample = sourceStream.WaveFormat.Channels * sourceStream.WaveFormat.BitsPerSample / 8;

            length = SourceToDest(sourceStream.Length);
            position = 0;
        }

        private long SourceToDest(long sourceBytes)
        {
            return (sourceBytes / sourceBytesPerSample) * destBytesPerSample;
        }

        private long DestToSource(long destBytes)
        {
            return (destBytes / destBytesPerSample) * sourceBytesPerSample;
        }
        public WaveChannel32(WaveStream sourceStream)
            :
            this(sourceStream, 1.0f, 0.0f)
        {
        }
        public override int BlockAlign
        {
            get
            {
                return (int)SourceToDest(sourceStream.BlockAlign);
            }
        }
        public override long Length
        {
            get
            {
                return length;
            }
        }
        public override long Position
        {
            get
            {
                return position;
            }
            set
            {
                lock (lockObject)
                {
                    // make sure we don't get out of sync
                    value -= (value % BlockAlign);
                    if (value < 0)
                    {
                        sourceStream.Position = 0;
                    }
                    else
                    {
                        sourceStream.Position = DestToSource(value);
                    }
                    // source stream may not have accepted the reposition we gave it
                    position = SourceToDest(sourceStream.Position);
                }
            }
        }
        public override int Read(byte[] destBuffer, int offset, int numBytes)
        {
            lock (lockObject)
            {
                int bytesWritten = 0;
                WaveBuffer destWaveBuffer = new WaveBuffer(destBuffer);

                // 1. fill with silence
                if (position < 0)
                {
                    bytesWritten = (int)Math.Min(numBytes, 0 - position);
                    for (int n = 0; n < bytesWritten; n++)
                        destBuffer[n + offset] = 0;
                }
                if (bytesWritten < numBytes)
                {
                    this.sampleProvider.LoadNextChunk(sourceStream, (numBytes - bytesWritten) / 8);
                    float left, right;

                    int outIndex = (offset / 4) + bytesWritten / 4;
                    while (this.sampleProvider.GetNextSample(out left, out right) && bytesWritten < numBytes)
                    {
                        // implement better panning laws. 
                        left = (pan <= 0) ? left : (left * (1 - pan) / 2.0f);
                        right = (pan >= 0) ? right : (right * (pan + 1) / 2.0f);
                        left *= volume;
                        right *= volume;
                        destWaveBuffer.FloatBuffer[outIndex++] = left;
                        destWaveBuffer.FloatBuffer[outIndex++] = right;
                        bytesWritten += 8;
                        if (Sample != null) RaiseSample(left, right);
                    }
                }
                // 3. Fill out with zeroes
                if (PadWithZeroes && bytesWritten < numBytes)
                {
                    Array.Clear(destBuffer, offset + bytesWritten, numBytes - bytesWritten);
                    bytesWritten = numBytes;
                }
                position += bytesWritten;
                return bytesWritten;
            }
        }
        public bool PadWithZeroes { get; set; }
        public override WaveFormat WaveFormat
        {
            get
            {
                return waveFormat;
            }
        }
        public float Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        public float Pan
        {
            get { return pan; }
            set { pan = value; }
        }
        public override bool HasData(int count)
        {
            // Check whether the source stream has data.
            bool sourceHasData = sourceStream.HasData(count);

            if (sourceHasData)
            {
                if (position + count < 0)
                    return false;
                return (position < length) && (volume != 0);
            }
            return false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sourceStream != null)
                {
                    sourceStream.Dispose();
                    sourceStream = null;
                }
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, "WaveChannel32 was not Disposed");
            }
            base.Dispose(disposing);
        }
        public event EventHandler<SampleEventArgs> Sample;
        private SampleEventArgs sampleEventArgs = new SampleEventArgs(0, 0);
        private void RaiseSample(float left, float right)
        {
            sampleEventArgs.Left = left;
            sampleEventArgs.Right = right;
            Sample(this, sampleEventArgs);
        }
    }
    public interface ISampleNotifier
    {
        event EventHandler<SampleEventArgs> Sample;
    }
    public class SampleEventArgs : EventArgs
    {
        public float Left { get; set; }
        public float Right { get; set; }

        public SampleEventArgs(float left, float right)
        {
            this.Left = left;
            this.Right = right;
        }
    }
    interface ISampleChunkConverter
    {
        bool Supports(WaveFormat format);
        void LoadNextChunk(IWaveProvider sourceProvider, int samplePairsRequired);
        bool GetNextSample(out float sampleLeft, out float sampleRight);
    }
    class WaveFileChunkReader
    {
        private WaveFormat waveFormat;
        private long dataChunkPosition;
        private long dataChunkLength;
        private List<RiffChunk> riffChunks;
        private readonly bool strictMode;
        private bool isRf64;
        private readonly bool storeAllChunks;
        private long riffSize;

        public WaveFileChunkReader()
        {
            storeAllChunks = true;
            strictMode = false;
        }

        public void ReadWaveHeader(Stream stream)
        {
            this.dataChunkPosition = -1;
            this.waveFormat = null;
            this.riffChunks = new List<RiffChunk>();
            this.dataChunkLength = 0;

            var br = new BinaryReader(stream);
            ReadRiffHeader(br);
            this.riffSize = br.ReadUInt32(); // read the file size (minus 8 bytes)

            if (br.ReadInt32() != ChunkIdentifier.ChunkIdentifierToInt32("WAVE"))
            {
                throw new FormatException("Not a WAVE file - no WAVE header");
            }

            if (isRf64)
            {
                ReadDs64Chunk(br);
            }

            int dataChunkId = ChunkIdentifier.ChunkIdentifierToInt32("data");
            int formatChunkId = ChunkIdentifier.ChunkIdentifierToInt32("fmt ");

            // sometimes a file has more data than is specified after the RIFF header
            long stopPosition = Math.Min(riffSize + 8, stream.Length);

            // this -8 is so we can be sure that there are at least 8 bytes for a chunk id and length
            while (stream.Position <= stopPosition - 8)
            {
                Int32 chunkIdentifier = br.ReadInt32();
                var chunkLength = br.ReadUInt32();
                if (chunkIdentifier == dataChunkId)
                {
                    dataChunkPosition = stream.Position;
                    if (!isRf64) // we already know the dataChunkLength if this is an RF64 file
                    {
                        dataChunkLength = chunkLength;
                    }
                    stream.Position += chunkLength;
                }
                else if (chunkIdentifier == formatChunkId)
                {
                    if (chunkLength > Int32.MaxValue)
                        throw new InvalidDataException(string.Format("Format chunk length must be between 0 and {0}.", Int32.MaxValue));
                    waveFormat = WaveFormat.FromFormatChunk(br, (int)chunkLength);
                }
                else
                {
                    // check for invalid chunk length
                    if (chunkLength > stream.Length - stream.Position)
                    {
                        if (strictMode)
                        {
                            Debug.Assert(false, String.Format("Invalid chunk length {0}, pos: {1}. length: {2}",
                                chunkLength, stream.Position, stream.Length));
                        }
                        // an exception will be thrown further down if we haven't got a format and data chunk yet,
                        // otherwise we will tolerate this file despite it having corrupt data at the end
                        break;
                    }
                    if (storeAllChunks)
                    {
                        if (chunkLength > Int32.MaxValue)
                            throw new InvalidDataException(string.Format("RiffChunk chunk length must be between 0 and {0}.", Int32.MaxValue));
                        riffChunks.Add(GetRiffChunk(stream, chunkIdentifier, (int)chunkLength));
                    }
                    stream.Position += chunkLength;
                }
            }

            if (waveFormat == null)
            {
                throw new FormatException("Invalid WAV file - No fmt chunk found");
            }
            if (dataChunkPosition == -1)
            {
                throw new FormatException("Invalid WAV file - No data chunk found");
            }
        }
        private void ReadDs64Chunk(BinaryReader reader)
        {
            int ds64ChunkId = ChunkIdentifier.ChunkIdentifierToInt32("ds64");
            int chunkId = reader.ReadInt32();
            if (chunkId != ds64ChunkId)
            {
                throw new FormatException("Invalid RF64 WAV file - No ds64 chunk found");
            }
            int chunkSize = reader.ReadInt32();
            this.riffSize = reader.ReadInt64();
            this.dataChunkLength = reader.ReadInt64();
            long sampleCount = reader.ReadInt64(); // replaces the value in the fact chunk
            reader.ReadBytes(chunkSize - 24); // get to the end of this chunk (should parse extra stuff later)
        }

        private static RiffChunk GetRiffChunk(Stream stream, Int32 chunkIdentifier, Int32 chunkLength)
        {
            return new RiffChunk(chunkIdentifier, chunkLength, stream.Position);
        }

        private void ReadRiffHeader(BinaryReader br)
        {
            int header = br.ReadInt32();
            if (header == ChunkIdentifier.ChunkIdentifierToInt32("RF64"))
            {
                this.isRf64 = true;
            }
            else if (header != ChunkIdentifier.ChunkIdentifierToInt32("RIFF"))
            {
                throw new FormatException("Not a WAVE file - no RIFF header");
            }
        }
        public WaveFormat WaveFormat { get { return this.waveFormat; } }
        public long DataChunkPosition { get { return this.dataChunkPosition; } }
        public long DataChunkLength { get { return this.dataChunkLength; } }
        public List<RiffChunk> RiffChunks { get { return this.riffChunks; } }
    }
    public class ChunkIdentifier
    {
        public static int ChunkIdentifierToInt32(string s)
        {
            if (s.Length != 4) throw new ArgumentException("Must be a four character string");
            var bytes = Encoding.UTF8.GetBytes(s);
            if (bytes.Length != 4) throw new ArgumentException("Must encode to exactly four bytes");
            return BitConverter.ToInt32(bytes, 0);
        }
    }
    public class WaveFileReader : WaveStream
    {
        private readonly WaveFormat waveFormat;
        private readonly bool ownInput;
        private readonly long dataPosition;
        private readonly long dataChunkLength;
        private readonly List<RiffChunk> chunks = new List<RiffChunk>();
        private readonly object lockObject = new object();
        private Stream waveStream;
        public WaveFileReader(String waveFile) :
            this(File.OpenRead(waveFile))
        {
            ownInput = true;
        }
        public WaveFileReader(Stream inputStream)
        {
            this.waveStream = inputStream;
            var chunkReader = new WaveFileChunkReader();
            chunkReader.ReadWaveHeader(inputStream);
            this.waveFormat = chunkReader.WaveFormat;
            this.dataPosition = chunkReader.DataChunkPosition;
            this.dataChunkLength = chunkReader.DataChunkLength;
            this.chunks = chunkReader.RiffChunks;
            Position = 0;
        }

        public List<RiffChunk> ExtraChunks
        {
            get
            {
                return chunks;
            }
        }
        public byte[] GetChunkData(RiffChunk chunk)
        {
            long oldPosition = waveStream.Position;
            waveStream.Position = chunk.StreamPosition;
            byte[] data = new byte[chunk.Length];
            waveStream.Read(data, 0, data.Length);
            waveStream.Position = oldPosition;
            return data;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources.
                if (waveStream != null)
                {
                    // only dispose our source if we created it
                    if (ownInput)
                    {
                        waveStream.Close();
                    }
                    waveStream = null;
                }
            }
            else
            {
                System.Diagnostics.Debug.Assert(false, "WaveFileReader was not disposed");
            }
            base.Dispose(disposing);
        }
        public override WaveFormat WaveFormat
        {
            get
            {
                return waveFormat;
            }
        }
        public override long Length
        {
            get
            {
                return dataChunkLength;
            }
        }
        public long SampleCount
        {
            get
            {
                if (waveFormat.Encoding == WaveFormatEncoding.Pcm ||
                    waveFormat.Encoding == WaveFormatEncoding.Extensible ||
                    waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
                {
                    return dataChunkLength / BlockAlign;
                }
                // n.b. if there is a fact chunk, you can use that to get the number of samples
                throw new InvalidOperationException("Sample count is calculated only for the standard encodings");
            }
        }

        public override long Position
        {
            get
            {
                return waveStream.Position - dataPosition;
            }
            set
            {
                lock (lockObject)
                {
                    value = Math.Min(value, Length);
                    // make sure we don't get out of sync
                    value -= (value % waveFormat.BlockAlign);
                    waveStream.Position = value + dataPosition;
                }
            }
        }

        public override int Read(byte[] array, int offset, int count)
        {
            if (count % waveFormat.BlockAlign != 0)
            {
                throw new ArgumentException(String.Format("Must read complete blocks: requested {0}, block align is {1}", count, this.WaveFormat.BlockAlign));
            }
            lock (lockObject)
            {
                // sometimes there is more junk at the end of the file past the data chunk
                if (Position + count > dataChunkLength)
                {
                    count = (int)(dataChunkLength - Position);
                }
                return waveStream.Read(array, offset, count);
            }
        }
        public float[] ReadNextSampleFrame()
        {
            switch (waveFormat.Encoding)
            {
                case WaveFormatEncoding.Pcm:
                case WaveFormatEncoding.IeeeFloat:
                case WaveFormatEncoding.Extensible: // n.b. not necessarily PCM, should probably write more code to handle this case
                    break;
                default:
                    throw new InvalidOperationException("Only 16, 24 or 32 bit PCM or IEEE float audio data supported");
            }
            var sampleFrame = new float[waveFormat.Channels];
            int bytesToRead = waveFormat.Channels * (waveFormat.BitsPerSample / 8);
            byte[] raw = new byte[bytesToRead];
            int bytesRead = Read(raw, 0, bytesToRead);
            if (bytesRead == 0) return null; // end of file
            if (bytesRead < bytesToRead) throw new InvalidDataException("Unexpected end of file");
            int offset = 0;
            for (int channel = 0; channel < waveFormat.Channels; channel++)
            {
                if (waveFormat.BitsPerSample == 16)
                {
                    sampleFrame[channel] = BitConverter.ToInt16(raw, offset) / 32768f;
                    offset += 2;
                }
                else if (waveFormat.BitsPerSample == 24)
                {
                    sampleFrame[channel] = (((sbyte)raw[offset + 2] << 16) | (raw[offset + 1] << 8) | raw[offset]) / 8388608f;
                    offset += 3;
                }
                else if (waveFormat.BitsPerSample == 32 && waveFormat.Encoding == WaveFormatEncoding.IeeeFloat)
                {
                    sampleFrame[channel] = BitConverter.ToSingle(raw, offset);
                    offset += 4;
                }
                else if (waveFormat.BitsPerSample == 32)
                {
                    sampleFrame[channel] = BitConverter.ToInt32(raw, offset) / (Int32.MaxValue + 1f);
                    offset += 4;
                }
                else
                {
                    throw new InvalidOperationException("Unsupported bit depth");
                }
            }
            return sampleFrame;
        }
        [Obsolete("Use ReadNextSampleFrame instead (this version does not support stereo properly)")]
        public bool TryReadFloat(out float sampleValue)
        {
            var sf = ReadNextSampleFrame();
            sampleValue = sf != null ? sf[0] : 0;
            return sf != null;
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class WaveFormat
    {
        protected WaveFormatEncoding waveFormatTag;
        protected short channels;
        protected int sampleRate;
        protected int averageBytesPerSecond;
        protected short blockAlign;
        protected short bitsPerSample;
        protected short extraSize;
        public WaveFormat() : this(44100, 16, 2)
        {

        }
        public WaveFormat(int sampleRate, int channels)
            : this(sampleRate, 16, channels)
        {
        }
        public int ConvertLatencyToByteSize(int milliseconds)
        {
            int bytes = (int)((AverageBytesPerSecond / 1000.0) * milliseconds);
            if ((bytes % BlockAlign) != 0)
            {
                // Return the upper BlockAligned
                bytes = bytes + BlockAlign - (bytes % BlockAlign);
            }
            return bytes;
        }
        public static WaveFormat CreateCustomFormat(WaveFormatEncoding tag, int sampleRate, int channels, int averageBytesPerSecond, int blockAlign, int bitsPerSample)
        {
            WaveFormat waveFormat = new WaveFormat();
            waveFormat.waveFormatTag = tag;
            waveFormat.channels = (short)channels;
            waveFormat.sampleRate = sampleRate;
            waveFormat.averageBytesPerSecond = averageBytesPerSecond;
            waveFormat.blockAlign = (short)blockAlign;
            waveFormat.bitsPerSample = (short)bitsPerSample;
            waveFormat.extraSize = 0;
            return waveFormat;
        }
        public static WaveFormat CreateALawFormat(int sampleRate, int channels)
        {
            return CreateCustomFormat(WaveFormatEncoding.ALaw, sampleRate, channels, sampleRate * channels, channels, 8);
        }
        public static WaveFormat CreateMuLawFormat(int sampleRate, int channels)
        {
            return CreateCustomFormat(WaveFormatEncoding.MuLaw, sampleRate, channels, sampleRate * channels, channels, 8);
        }
        public WaveFormat(int rate, int bits, int channels)
        {
            if (channels < 1)
            {
                throw new ArgumentOutOfRangeException("channels", "Channels must be 1 or greater");
            }
            // minimum 16 bytes, sometimes 18 for PCM
            this.waveFormatTag = WaveFormatEncoding.Pcm;
            this.channels = (short)channels;
            this.sampleRate = rate;
            this.bitsPerSample = (short)bits;
            this.extraSize = 0;

            this.blockAlign = (short)(channels * (bits / 8));
            this.averageBytesPerSecond = this.sampleRate * this.blockAlign;
        }
        public static WaveFormat CreateIeeeFloatWaveFormat(int sampleRate, int channels)
        {
            WaveFormat wf = new WaveFormat();
            wf.waveFormatTag = WaveFormatEncoding.IeeeFloat;
            wf.channels = (short)channels;
            wf.bitsPerSample = 32;
            wf.sampleRate = sampleRate;
            wf.blockAlign = (short)(4 * channels);
            wf.averageBytesPerSecond = sampleRate * wf.blockAlign;
            wf.extraSize = 0;
            return wf;
        }
        public static WaveFormat MarshalFromPtr(IntPtr pointer)
        {
            WaveFormat waveFormat = (WaveFormat)Marshal.PtrToStructure(pointer, typeof(WaveFormat));
            switch (waveFormat.Encoding)
            {
                case WaveFormatEncoding.Pcm:
                    // can't rely on extra size even being there for PCM so blank it to avoid reading
                    // corrupt data
                    waveFormat.extraSize = 0;
                    break;
                case WaveFormatEncoding.Extensible:
                    waveFormat = (WaveFormatExtensible)Marshal.PtrToStructure(pointer, typeof(WaveFormatExtensible));
                    break;
                case WaveFormatEncoding.Adpcm:
                    waveFormat = (AdpcmWaveFormat)Marshal.PtrToStructure(pointer, typeof(AdpcmWaveFormat));
                    break;
                case WaveFormatEncoding.Gsm610:
                    waveFormat = (Gsm610WaveFormat)Marshal.PtrToStructure(pointer, typeof(Gsm610WaveFormat));
                    break;
                default:
                    if (waveFormat.ExtraSize > 0)
                    {
                        waveFormat = (WaveFormatExtraData)Marshal.PtrToStructure(pointer, typeof(WaveFormatExtraData));
                    }
                    break;
            }
            return waveFormat;
        }
        public static IntPtr MarshalToPtr(WaveFormat format)
        {
            int formatSize = Marshal.SizeOf(format);
            IntPtr formatPointer = Marshal.AllocHGlobal(formatSize);
            Marshal.StructureToPtr(format, formatPointer, false);
            return formatPointer;
        }
        public static WaveFormat FromFormatChunk(BinaryReader br, int formatChunkLength)
        {
            var waveFormat = new WaveFormatExtraData();
            waveFormat.ReadWaveFormat(br, formatChunkLength);
            waveFormat.ReadExtraData(br);
            return waveFormat;
        }

        private void ReadWaveFormat(BinaryReader br, int formatChunkLength)
        {
            if (formatChunkLength < 16)
                throw new InvalidDataException("Invalid WaveFormat Structure");
            this.waveFormatTag = (WaveFormatEncoding)br.ReadUInt16();
            this.channels = br.ReadInt16();
            this.sampleRate = br.ReadInt32();
            this.averageBytesPerSecond = br.ReadInt32();
            this.blockAlign = br.ReadInt16();
            this.bitsPerSample = br.ReadInt16();
            if (formatChunkLength > 16)
            {
                this.extraSize = br.ReadInt16();
                if (this.extraSize != formatChunkLength - 18)
                {
                    Debug.WriteLine("Format chunk mismatch");
                    this.extraSize = (short)(formatChunkLength - 18);
                }
            }
        }
        public WaveFormat(BinaryReader br)
        {
            int formatChunkLength = br.ReadInt32();
            this.ReadWaveFormat(br, formatChunkLength);
        }
        public override string ToString()
        {
            switch (this.waveFormatTag)
            {
                case WaveFormatEncoding.Pcm:
                case WaveFormatEncoding.Extensible:
                    // extensible just has some extra bits after the PCM header
                    return String.Format("{0} bit PCM: {1}kHz {2} channels",
                        bitsPerSample, sampleRate / 1000, channels);
                default:
                    return this.waveFormatTag.ToString();
            }
        }
        public override bool Equals(object obj)
        {
            WaveFormat other = obj as WaveFormat;
            if (other != null)
            {
                return waveFormatTag == other.waveFormatTag &&
                    channels == other.channels &&
                    sampleRate == other.sampleRate &&
                    averageBytesPerSecond == other.averageBytesPerSecond &&
                    blockAlign == other.blockAlign &&
                    bitsPerSample == other.bitsPerSample;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return (int)waveFormatTag ^
                (int)channels ^
                sampleRate ^
                averageBytesPerSecond ^
                (int)blockAlign ^
                (int)bitsPerSample;
        }
        public WaveFormatEncoding Encoding
        {
            get
            {
                return waveFormatTag;
            }
        }
        public virtual void Serialize(BinaryWriter writer)
        {
            writer.Write((int)(18 + extraSize)); // wave format length
            writer.Write((short)Encoding);
            writer.Write((short)Channels);
            writer.Write((int)SampleRate);
            writer.Write((int)AverageBytesPerSecond);
            writer.Write((short)BlockAlign);
            writer.Write((short)BitsPerSample);
            writer.Write((short)extraSize);
        }
        public int Channels
        {
            get
            {
                return channels;
            }
        }
        public int SampleRate
        {
            get
            {
                return sampleRate;
            }
        }
        public int AverageBytesPerSecond
        {
            get
            {
                return averageBytesPerSecond;
            }
        }
        public virtual int BlockAlign
        {
            get
            {
                return blockAlign;
            }
        }
        public int BitsPerSample
        {
            get
            {
                return bitsPerSample;
            }
        }
        public int ExtraSize
        {
            get
            {
                return extraSize;
            }
        }
    }
    public enum WaveFormatEncoding : ushort
    {
        Unknown = 0x0000,
        Pcm = 0x0001,
        Adpcm = 0x0002,
        IeeeFloat = 0x0003,
        Vselp = 0x0004,
        IbmCvsd = 0x0005,
        ALaw = 0x0006,
        MuLaw = 0x0007,
        Dts = 0x0008,
        Drm = 0x0009,
        WmaVoice9 = 0x000A,
        OkiAdpcm = 0x0010,
        DviAdpcm = 0x0011,
        ImaAdpcm = DviAdpcm,
        MediaspaceAdpcm = 0x0012,
        SierraAdpcm = 0x0013,
        G723Adpcm = 0x0014,
        DigiStd = 0x0015,
        DigiFix = 0x0016,
        DialogicOkiAdpcm = 0x0017,
        MediaVisionAdpcm = 0x0018,
        CUCodec = 0x0019,
        YamahaAdpcm = 0x0020,
        SonarC = 0x0021,
        DspGroupTrueSpeech = 0x0022,
        EchoSpeechCorporation1 = 0x0023,
        AudioFileAf36 = 0x0024,
        Aptx = 0x0025,
        AudioFileAf10 = 0x0026,
        Prosody1612 = 0x0027,
        Lrc = 0x0028,
        DolbyAc2 = 0x0030,
        Gsm610 = 0x0031,
        MsnAudio = 0x0032,
        AntexAdpcme = 0x0033,
        ControlResVqlpc = 0x0034,
        DigiReal = 0x0035,
        DigiAdpcm = 0x0036,
        ControlResCr10 = 0x0037,
        WAVE_FORMAT_NMS_VBXADPCM = 0x0038, // Natural MicroSystems 
        WAVE_FORMAT_CS_IMAADPCM = 0x0039, // Crystal Semiconductor IMA ADPCM 
        WAVE_FORMAT_ECHOSC3 = 0x003A, // Echo Speech Corporation 
        WAVE_FORMAT_ROCKWELL_ADPCM = 0x003B, // Rockwell International 
        WAVE_FORMAT_ROCKWELL_DIGITALK = 0x003C, // Rockwell International 
        WAVE_FORMAT_XEBEC = 0x003D, // Xebec Multimedia Solutions Limited 
        WAVE_FORMAT_G721_ADPCM = 0x0040, // Antex Electronics Corporation 
        WAVE_FORMAT_G728_CELP = 0x0041, // Antex Electronics Corporation 
        WAVE_FORMAT_MSG723 = 0x0042, // Microsoft Corporation 
        Mpeg = 0x0050,
        WAVE_FORMAT_RT24 = 0x0052, // InSoft, Inc. 
        WAVE_FORMAT_PAC = 0x0053, // InSoft, Inc. 
        MpegLayer3 = 0x0055,
        WAVE_FORMAT_LUCENT_G723 = 0x0059, // Lucent Technologies 
        WAVE_FORMAT_CIRRUS = 0x0060, // Cirrus Logic 
        WAVE_FORMAT_ESPCM = 0x0061, // ESS Technology 
        WAVE_FORMAT_VOXWARE = 0x0062, // Voxware Inc 
        WAVE_FORMAT_CANOPUS_ATRAC = 0x0063, // Canopus, co., Ltd. 
        WAVE_FORMAT_G726_ADPCM = 0x0064, // APICOM 
        WAVE_FORMAT_G722_ADPCM = 0x0065, // APICOM 
        WAVE_FORMAT_DSAT_DISPLAY = 0x0067, // Microsoft Corporation 
        WAVE_FORMAT_VOXWARE_BYTE_ALIGNED = 0x0069, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_AC8 = 0x0070, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_AC10 = 0x0071, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_AC16 = 0x0072, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_AC20 = 0x0073, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_RT24 = 0x0074, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_RT29 = 0x0075, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_RT29HW = 0x0076, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_VR12 = 0x0077, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_VR18 = 0x0078, // Voxware Inc 
        WAVE_FORMAT_VOXWARE_TQ40 = 0x0079, // Voxware Inc 
        WAVE_FORMAT_SOFTSOUND = 0x0080, // Softsound, Ltd. 
        WAVE_FORMAT_VOXWARE_TQ60 = 0x0081, // Voxware Inc 
        WAVE_FORMAT_MSRT24 = 0x0082, // Microsoft Corporation 
        WAVE_FORMAT_G729A = 0x0083, // AT&T Labs, Inc. 
        WAVE_FORMAT_MVI_MVI2 = 0x0084, // Motion Pixels 
        WAVE_FORMAT_DF_G726 = 0x0085, // DataFusion Systems (Pty) (Ltd) 
        WAVE_FORMAT_DF_GSM610 = 0x0086, // DataFusion Systems (Pty) (Ltd) 
        WAVE_FORMAT_ISIAUDIO = 0x0088, // Iterated Systems, Inc. 
        WAVE_FORMAT_ONLIVE = 0x0089, // OnLive! Technologies, Inc. 
        WAVE_FORMAT_SBC24 = 0x0091, // Siemens Business Communications Sys 
        WAVE_FORMAT_DOLBY_AC3_SPDIF = 0x0092, // Sonic Foundry 
        WAVE_FORMAT_MEDIASONIC_G723 = 0x0093, // MediaSonic 
        WAVE_FORMAT_PROSODY_8KBPS = 0x0094, // Aculab plc 
        WAVE_FORMAT_ZYXEL_ADPCM = 0x0097, // ZyXEL Communications, Inc. 
        WAVE_FORMAT_PHILIPS_LPCBB = 0x0098, // Philips Speech Processing 
        WAVE_FORMAT_PACKED = 0x0099, // Studer Professional Audio AG 
        WAVE_FORMAT_MALDEN_PHONYTALK = 0x00A0, // Malden Electronics Ltd. 
        Gsm = 0x00A1,
        G729 = 0x00A2,
        G723 = 0x00A3,
        Acelp = 0x00A4,
        RawAac = 0x00FF,
        WAVE_FORMAT_RHETOREX_ADPCM = 0x0100, // Rhetorex Inc. 
        WAVE_FORMAT_IRAT = 0x0101, // BeCubed Software Inc. 
        WAVE_FORMAT_VIVO_G723 = 0x0111, // Vivo Software 
        WAVE_FORMAT_VIVO_SIREN = 0x0112, // Vivo Software 
        WAVE_FORMAT_DIGITAL_G723 = 0x0123, // Digital Equipment Corporation 
        WAVE_FORMAT_SANYO_LD_ADPCM = 0x0125, // Sanyo Electric Co., Ltd. 
        WAVE_FORMAT_SIPROLAB_ACEPLNET = 0x0130, // Sipro Lab Telecom Inc. 
        WAVE_FORMAT_SIPROLAB_ACELP4800 = 0x0131, // Sipro Lab Telecom Inc. 
        WAVE_FORMAT_SIPROLAB_ACELP8V3 = 0x0132, // Sipro Lab Telecom Inc. 
        WAVE_FORMAT_SIPROLAB_G729 = 0x0133, // Sipro Lab Telecom Inc. 
        WAVE_FORMAT_SIPROLAB_G729A = 0x0134, // Sipro Lab Telecom Inc. 
        WAVE_FORMAT_SIPROLAB_KELVIN = 0x0135, // Sipro Lab Telecom Inc. 
        WAVE_FORMAT_G726ADPCM = 0x0140, // Dictaphone Corporation 
        WAVE_FORMAT_QUALCOMM_PUREVOICE = 0x0150, // Qualcomm, Inc. 
        WAVE_FORMAT_QUALCOMM_HALFRATE = 0x0151, // Qualcomm, Inc. 
        WAVE_FORMAT_TUBGSM = 0x0155, // Ring Zero Systems, Inc. 
        WAVE_FORMAT_MSAUDIO1 = 0x0160, // Microsoft Corporation
        WindowsMediaAudio = 0x0161,
        WindowsMediaAudioProfessional = 0x0162,
        WindowsMediaAudioLosseless = 0x0163,
        WindowsMediaAudioSpdif = 0x0164,
        WAVE_FORMAT_UNISYS_NAP_ADPCM = 0x0170, // Unisys Corp. 
        WAVE_FORMAT_UNISYS_NAP_ULAW = 0x0171, // Unisys Corp. 
        WAVE_FORMAT_UNISYS_NAP_ALAW = 0x0172, // Unisys Corp. 
        WAVE_FORMAT_UNISYS_NAP_16K = 0x0173, // Unisys Corp. 
        WAVE_FORMAT_CREATIVE_ADPCM = 0x0200, // Creative Labs, Inc 
        WAVE_FORMAT_CREATIVE_FASTSPEECH8 = 0x0202, // Creative Labs, Inc 
        WAVE_FORMAT_CREATIVE_FASTSPEECH10 = 0x0203, // Creative Labs, Inc 
        WAVE_FORMAT_UHER_ADPCM = 0x0210, // UHER informatic GmbH 
        WAVE_FORMAT_QUARTERDECK = 0x0220, // Quarterdeck Corporation 
        WAVE_FORMAT_ILINK_VC = 0x0230, // I-link Worldwide 
        WAVE_FORMAT_RAW_SPORT = 0x0240, // Aureal Semiconductor 
        WAVE_FORMAT_ESST_AC3 = 0x0241, // ESS Technology, Inc. 
        WAVE_FORMAT_IPI_HSX = 0x0250, // Interactive Products, Inc. 
        WAVE_FORMAT_IPI_RPELP = 0x0251, // Interactive Products, Inc. 
        WAVE_FORMAT_CS2 = 0x0260, // Consistent Software 
        WAVE_FORMAT_SONY_SCX = 0x0270, // Sony Corp. 
        WAVE_FORMAT_FM_TOWNS_SND = 0x0300, // Fujitsu Corp. 
        WAVE_FORMAT_BTV_DIGITAL = 0x0400, // Brooktree Corporation 
        WAVE_FORMAT_QDESIGN_MUSIC = 0x0450, // QDesign Corporation 
        WAVE_FORMAT_VME_VMPCM = 0x0680, // AT&T Labs, Inc. 
        WAVE_FORMAT_TPC = 0x0681, // AT&T Labs, Inc. 
        WAVE_FORMAT_OLIGSM = 0x1000, // Ing C. Olivetti & C., S.p.A. 
        WAVE_FORMAT_OLIADPCM = 0x1001, // Ing C. Olivetti & C., S.p.A. 
        WAVE_FORMAT_OLICELP = 0x1002, // Ing C. Olivetti & C., S.p.A. 
        WAVE_FORMAT_OLISBC = 0x1003, // Ing C. Olivetti & C., S.p.A. 
        WAVE_FORMAT_OLIOPR = 0x1004, // Ing C. Olivetti & C., S.p.A. 
        WAVE_FORMAT_LH_CODEC = 0x1100, // Lernout & Hauspie 
        WAVE_FORMAT_NORRIS = 0x1400, // Norris Communications, Inc. 
        WAVE_FORMAT_SOUNDSPACE_MUSICOMPRESS = 0x1500, // AT&T Labs, Inc. 
        MPEG_ADTS_AAC = 0x1600,
        MPEG_RAW_AAC = 0x1601,
        MPEG_LOAS = 0x1602,
        NOKIA_MPEG_ADTS_AAC = 0x1608,
        NOKIA_MPEG_RAW_AAC = 0x1609,
        VODAFONE_MPEG_ADTS_AAC = 0x160A,
        VODAFONE_MPEG_RAW_AAC = 0x160B,
        MPEG_HEAAC = 0x1610,
        WAVE_FORMAT_DVM = 0x2000, // FAST Multimedia AG 
        Vorbis1 = 0x674f,
        Vorbis2 = 0x6750,
        Vorbis3 = 0x6751,
        Vorbis1P = 0x676f,
        Vorbis2P = 0x6770,
        Vorbis3P = 0x6771,
        Extensible = 0xFFFE, // Microsoft 
        WAVE_FORMAT_DEVELOPMENT = 0xFFFF,
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class WaveFormatExtensible : WaveFormat
    {
        short wValidBitsPerSample; // bits of precision, or is wSamplesPerBlock if wBitsPerSample==0
        int dwChannelMask; // which channels are present in stream
        Guid subFormat;
        WaveFormatExtensible()
        {
        }
        public WaveFormatExtensible(int rate, int bits, int channels)
            : base(rate, bits, channels)
        {
            waveFormatTag = WaveFormatEncoding.Extensible;
            extraSize = 22;
            wValidBitsPerSample = (short)bits;
            for (int n = 0; n < channels; n++)
            {
                dwChannelMask |= (1 << n);
            }
            if (bits == 32)
            {
                // KSDATAFORMAT_SUBTYPE_IEEE_FLOAT
                subFormat = AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT; // new Guid("00000003-0000-0010-8000-00aa00389b71");
            }
            else
            {
                // KSDATAFORMAT_SUBTYPE_PCM
                subFormat = AudioMediaSubtypes.MEDIASUBTYPE_PCM; // new Guid("00000001-0000-0010-8000-00aa00389b71");
            }

        }
        public WaveFormat ToStandardWaveFormat()
        {
            if (subFormat == AudioMediaSubtypes.MEDIASUBTYPE_IEEE_FLOAT && bitsPerSample == 32)
                return CreateIeeeFloatWaveFormat(sampleRate, channels);
            if (subFormat == AudioMediaSubtypes.MEDIASUBTYPE_PCM)
                return new WaveFormat(sampleRate, bitsPerSample, channels);
            throw new InvalidOperationException("Not a recognised PCM or IEEE float format");
        }
        public Guid SubFormat { get { return subFormat; } }
        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(wValidBitsPerSample);
            writer.Write(dwChannelMask);
            byte[] guid = subFormat.ToByteArray();
            writer.Write(guid, 0, guid.Length);
        }
        public override string ToString()
        {
            return String.Format("{0} wBitsPerSample:{1} dwChannelMask:{2} subFormat:{3} extraSize:{4}",
                base.ToString(),
                wValidBitsPerSample,
                dwChannelMask,
                subFormat,
                extraSize);
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
    public class WaveFormatExtraData : WaveFormat
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        private byte[] extraData = new byte[100];

        public byte[] ExtraData { get { return extraData; } }
        internal WaveFormatExtraData()
        {
        }
        public WaveFormatExtraData(BinaryReader reader)
            : base(reader)
        {
            ReadExtraData(reader);
        }

        internal void ReadExtraData(BinaryReader reader)
        {
            if (this.extraSize > 0)
            {
                reader.Read(extraData, 0, extraSize);
            }
        }
        public override void Serialize(BinaryWriter writer)
        {
            base.Serialize(writer);
            if (extraSize > 0)
            {
                writer.Write(extraData, 0, extraSize);
            }
        }
    }
    public abstract class WaveStream : Stream, IWaveProvider
    {
        public abstract WaveFormat WaveFormat { get; }
        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return false; } }
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
                Position = offset;
            else if (origin == SeekOrigin.Current)
                Position += offset;
            else
                Position = Length + offset;
            return Position;
        }
        public override void SetLength(long length)
        {
            throw new NotSupportedException("Can't set length of a WaveFormatString");
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("Can't write to a WaveFormatString");
        }
        public virtual int BlockAlign
        {
            get
            {
                return WaveFormat.BlockAlign;
            }
        }
        public void Skip(int seconds)
        {
            long newPosition = Position + WaveFormat.AverageBytesPerSecond * seconds;
            if (newPosition > Length)
                Position = Length;
            else if (newPosition < 0)
                Position = 0;
            else
                Position = newPosition;
        }
        public virtual TimeSpan CurrentTime
        {
            get
            {
                return TimeSpan.FromSeconds((double)Position / WaveFormat.AverageBytesPerSecond);
            }
            set
            {
                Position = (long)(value.TotalSeconds * WaveFormat.AverageBytesPerSecond);
            }
        }
        public virtual TimeSpan TotalTime
        {
            get
            {

                return TimeSpan.FromSeconds((double)Length / WaveFormat.AverageBytesPerSecond);
            }
        }

        public virtual bool HasData(int count)
        {
            return Position < Length;
        }
    }
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public class AdpcmWaveFormat : WaveFormat
    {
        short samplesPerBlock;
        short numCoeff;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
        short[] coefficients;
        AdpcmWaveFormat() : this(8000, 1)
        {
        }
        public int SamplesPerBlock
        {
            get { return samplesPerBlock; }
        }
        public int NumCoefficients
        {
            get { return numCoeff; }
        }
        public short[] Coefficients
        {
            get { return coefficients; }
        }
        public AdpcmWaveFormat(int sampleRate, int channels) :
            base(sampleRate, 0, channels)
        {
            this.waveFormatTag = WaveFormatEncoding.Adpcm;
            this.extraSize = 32;
            switch (this.sampleRate)
            {
                case 8000:
                case 11025:
                    blockAlign = 256;
                    break;
                case 22050:
                    blockAlign = 512;
                    break;
                case 44100:
                default:
                    blockAlign = 1024;
                    break;
            }

            this.bitsPerSample = 4;
            this.samplesPerBlock = (short)((((blockAlign - (7 * channels)) * 8) / (bitsPerSample * channels)) + 2);
            this.averageBytesPerSecond =
                ((this.SampleRate * blockAlign) / samplesPerBlock);

            numCoeff = 7;
            coefficients = new short[14] {
                256,0,512,-256,0,0,192,64,240,0,460,-208,392,-232
            };
        }
        public override void Serialize(System.IO.BinaryWriter writer)
        {
            base.Serialize(writer);
            writer.Write(samplesPerBlock);
            writer.Write(numCoeff);
            foreach (short coefficient in coefficients)
            {
                writer.Write(coefficient);
            }
        }

        public override string ToString()
        {
            return String.Format("Microsoft ADPCM {0} Hz {1} channels {2} bits per sample {3} samples per block",
                this.SampleRate, this.channels, this.bitsPerSample, this.samplesPerBlock);
        }
    }
    */
}
