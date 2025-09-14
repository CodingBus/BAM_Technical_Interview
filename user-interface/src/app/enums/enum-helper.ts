export function getEnumName<T>(enumObj: T, value: number): string {
  return (enumObj as any)[value] ?? value.toString();
}