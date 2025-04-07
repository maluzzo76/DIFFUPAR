-- Monitorear el Uso de Memoria Antes y Después

SELECT 
    total_physical_memory_kb / 1024 AS TotalRAM_MB,
    available_physical_memory_kb / 1024 AS RAM_Disponible_MB,
    system_memory_state_desc 
FROM sys.dm_os_sys_memory;

-- O para ver la memoria específica de SQL Server:
SELECT 
    (physical_memory_in_use_kb / 1024) AS SQLServer_Memory_MB,
    (locked_page_allocations_kb / 1024) AS Locked_Memory_MB,
    (total_virtual_address_space_kb / 1024) AS VirtualMemory_MB
FROM sys.dm_os_process_memory;

-- Liberar memoria cache SP
DBCC FREESESSIONCACHE;

-- Guardar paginas en hd
CHECKPOINT;

--Liberar Memoria
DBCC FREEPROCCACHE;

--limpiar el buffer
DBCC DROPCLEANBUFFERS;